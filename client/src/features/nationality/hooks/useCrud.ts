import { useCallback, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '@/features/nationality/api/api.js';
import type { Nationality } from '@/features/nationality/types/nationality.js';
import { useDebounce } from '@/shared/hooks/useDebounce.js';
import { routes } from '@/shared/lib/constants/api.js';
import { IsCanceledError, IsError } from '@/shared/lib/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

export function useAll() {
  const { token } = useAuth();
  const navigate = useNavigate();

  const [nationalities, setNationalities] = useState<Nationality[]>([]);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(
    async (signal?: AbortSignal) => {
      if (!token) {
        return;
      }

      try {
        setIsFetching(true);
        const data = await api.all(token, signal);
        setNationalities(Array.isArray(data) ? data : []);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }
        const message = IsError(error) ? error.message : 'Failed to load nationalities.';
        navigate(routes.badRequest, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    },
    [token, navigate],
  );

  useEffect(() => {
    const controller = new AbortController();
    void fetchData(controller.signal);

    return () => controller.abort();
  }, [fetchData]);

  return { nationalities, isFetching, refetch: fetchData };
}
export function useSearchNationalities(allNationalities: Nationality[]) {
  const [searchTerm, setSearchTerm] = useState('');
  const debounced = useDebounce(searchTerm, 150);
  const [filteredNationalities, setFilteredNationalities] = useState<Nationality[]>([]);
  const [showDropdown, setShowDropdown] = useState(false);

  useEffect(() => {
    const term = debounced.trim().toLowerCase();
    if (!term) {
      setFilteredNationalities([]);
      setShowDropdown(false);

      return;
    }
    setFilteredNationalities(allNationalities.filter((n) => n.name.toLowerCase().includes(term)));
  }, [debounced, allNationalities]);

  const updateSearchTerm = useCallback((term: string) => {
    setSearchTerm(term);
    setShowDropdown(true);
  }, []);

  const selectNationality = useCallback((nationality: Nationality) => {
    setSearchTerm(nationality.name);
    setShowDropdown(false);
  }, []);

  const showDropdownOnFocus = useCallback(() => setShowDropdown(true), []);
  const hideDropdownOnBlur = useCallback(() => setShowDropdown(false), []);

  return {
    searchTerm,
    filteredNationalities,
    showDropdown,
    updateSearchTerm,
    selectNationality,
    showDropdownOnFocus,
    hideDropdownOnBlur,
  };
}

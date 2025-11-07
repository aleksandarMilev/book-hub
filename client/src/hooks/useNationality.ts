import { useCallback, useContext, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '../api/nationality/nationalityApi';
import type { Nationality } from '../api/nationality/types/nationality';
import { routes } from '../common/constants/api';
import { UserContext } from '../contexts/user/userContext';


export function useNationalities() {
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const [nationalities, setNationalities] = useState<Nationality[]>([]);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(async () => {
    if (!token) {
      return;
    }

    try {
      setIsFetching(true);
      const data = await api.all(token);
      setNationalities(data);
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Failed to load nationalities.';
      navigate(routes.badRequest, { state: { message } });
    } finally {
      setIsFetching(false);
    }
  }, [token, navigate]);

  useEffect(() => {
    void fetchData();
  }, [fetchData]);

  return { nationalities, isFetching, refetch: fetchData };
}

export function useSearchNationalities(allNationalities: Nationality[]) {
  const [searchTerm, setSearchTerm] = useState('');
  const [filteredNationalities, setFilteredNationalities] = useState<Nationality[]>([]);
  const [showDropdown, setShowDropdown] = useState(false);

  useEffect(() => {
    if (searchTerm.trim() === '') {
      setFilteredNationalities([]);
      return;
    }

    const lowerTerm = searchTerm.toLowerCase();
    const filtered = allNationalities.filter((n) => n.name.toLowerCase().includes(lowerTerm));

    setFilteredNationalities(filtered);
  }, [searchTerm, allNationalities]);

  const updateSearchTerm = useCallback((term: string) => {
    setSearchTerm(term);
    setShowDropdown(true);
  }, []);

  const selectNationality = useCallback((nationality: Nationality) => {
    setSearchTerm(nationality.name);
    setShowDropdown(false);
  }, []);

  const showDropdownOnFocus = useCallback(() => setShowDropdown(true), []);

  return {
    searchTerm,
    filteredNationalities,
    showDropdown,
    updateSearchTerm,
    selectNationality,
    showDropdownOnFocus,
  };
}

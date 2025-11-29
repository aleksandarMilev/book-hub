import { useCallback, useEffect, useState } from 'react';

import { ALL_NATIONALITIES, type Nationality } from '@/features/author/types/author.js';
import { useDebounce } from '@/shared/hooks/debounce/useDebounce.js';

export function useAll() {
  const [nationalities, setNationalities] = useState<Nationality[]>([]);
  const [isFetching, setIsFetching] = useState(false);

  useEffect(() => {
    setIsFetching(true);
    setTimeout(() => {
      setNationalities(ALL_NATIONALITIES);
      setIsFetching(false);
    }, 0);
  }, []);

  return { nationalities, isFetching };
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

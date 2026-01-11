import { useCallback, useEffect, useState } from 'react';

import { ALL_NATIONALITIES, type Nationality } from '@/features/author/types/author.js';
import { useDebounce } from '@/shared/hooks/debounce/useDebounce.js';
import { useTranslation } from 'react-i18next';

export const useAll = () => {
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
};

export const useSearchNationalities = (
  allNationalities: Nationality[],
  selectedId?: number | null,
) => {
  const { t } = useTranslation('authors');
  const [searchTerm, setSearchTerm] = useState('');
  const debounced = useDebounce(searchTerm, 150);
  const [filteredNationalities, setFilteredNationalities] = useState<Nationality[]>([]);
  const [showDropdown, setShowDropdown] = useState(false);

  useEffect(() => {
    if (selectedId == null) {
      setSearchTerm('');
      return;
    }

    const match = allNationalities.find((n) => n.id === selectedId);
    setSearchTerm(match?.name ?? '');
  }, [selectedId, allNationalities]);

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
    t,
    searchTerm,
    filteredNationalities,
    showDropdown,
    updateSearchTerm,
    selectNationality,
    showDropdownOnFocus,
    hideDropdownOnBlur,
  };
};

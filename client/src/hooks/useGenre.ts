import { useCallback, useContext, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '../api/genre/genreApi';
import { routes } from '../common/constants/api';
import { UserContext } from '../contexts/user/userContext';
import type { GenreName } from '../api/genre/types/genreName';
import type { GenreDetails } from '../api/genre/types/genreDetails';

export function useGenres() {
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const [genres, setGenres] = useState<GenreName[]>([]);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(async () => {
    if (!token) {
      return;
    }

    try {
      setIsFetching(true);
      const data = await api.all(token);
      setGenres(data);
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Failed to load genres.';
      navigate(routes.badRequest, { state: { message } });
    } finally {
      setIsFetching(false);
    }
  }, [token, navigate]);

  useEffect(() => {
    void fetchData();
  }, [fetchData]);

  return { genres, isFetching, refetch: fetchData };
}

export function useSearchGenres(genres: GenreName[], selectedGenres: GenreName[]) {
  const [searchTerm, setSearchTerm] = useState('');
  const [filteredGenres, setFilteredGenres] = useState<GenreName[]>([]);

  useEffect(() => {
    if (searchTerm.trim() === '') {
      setFilteredGenres([]);
      return;
    }

    const lowerTerm = searchTerm.toLowerCase();
    const filtered = genres.filter(
      (g) =>
        g.name.toLowerCase().includes(lowerTerm) &&
        !selectedGenres.some((selected) => selected.id === g.id),
    );

    setFilteredGenres(filtered);
  }, [searchTerm, genres, selectedGenres]);

  const updateSearchTerm = useCallback((term: string) => {
    setSearchTerm(term);
  }, []);

  return { searchTerm, filteredGenres, updateSearchTerm };
}

export function useDetails(id: number) {
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const [genre, setGenre] = useState<GenreDetails | null>(null);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(async () => {
    if (!token || !id) {
      return;
    }

    try {
      setIsFetching(true);
      const data = await api.details(id, token);
      setGenre(data);
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Failed to load genre details.';
      navigate(routes.badRequest, { state: { message } });
    } finally {
      setIsFetching(false);
    }
  }, [id, token, navigate]);

  useEffect(() => {
    void fetchData();
  }, [fetchData]);

  return { genre, isFetching, refetch: fetchData };
}

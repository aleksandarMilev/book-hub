import { useCallback, useContext, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '../api/genre/genreApi';
import type { Genre, GenreDetails } from '../api/genre/types/genre';
import { routes } from '../common/constants/api';
import { UserContext } from '../contexts/user/userContext';


export function useAll() {
  const navigate = useNavigate();
  const { token } = useContext(UserContext);

  const [genres, setGenres] = useState<Genre[]>([]);
  const [isFetching, setIsFetching] = useState<boolean>(false);

  const fetchData = useCallback(
    async (signal?: AbortSignal) => {
      if (!token) {
        return;
      }

      try {
        setIsFetching(true);

        const data = await api.all(token, signal);

        setGenres(data ?? []);
      } catch (error) {
        if (error instanceof DOMException && error.name === 'AbortError') {
          return;
        }

        const message = error instanceof Error ? error.message : 'Failed to load genres.';
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

  return { genres, isFetching, refetch: fetchData };
}

export function useSearch(genres: Genre[], selectedGenres: any[]) {
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [filteredGenres, setFilteredGenres] = useState<any[]>([]);

  useEffect(() => {
    if (searchTerm.trim() === '') {
      setFilteredGenres([]);
      return;
    }

    const lowerTerm = searchTerm.toLowerCase();
    const filtered = genres.filter(
      (g: any) =>
        g.name.toLowerCase().includes(lowerTerm) &&
        !selectedGenres.some((selected: any) => selected.id === g.id),
    );

    setFilteredGenres(filtered);
  }, [searchTerm, genres, selectedGenres]);

  const updateSearchTerm = useCallback((term: string) => {
    setSearchTerm(term);
  }, []);

  return { searchTerm, filteredGenres, updateSearchTerm };
}

export function useDetails(id: number) {
  const navigate = useNavigate();
  const { token } = useContext(UserContext);

  const [genre, setGenre] = useState<GenreDetails | null>(null);
  const [isFetching, setIsFetching] = useState<boolean>(false);

  const fetchData = useCallback(
    async (signal?: AbortSignal) => {
      if (!token || !id) {
        return;
      }

      try {
        setIsFetching(true);

        const data = await api.details(id, token, signal);

        setGenre(data);
      } catch (error) {
        if (error instanceof DOMException && error.name === 'AbortError') {
          return;
        }

        const message = error instanceof Error ? error.message : 'Failed to load genre details.';
        navigate(routes.badRequest, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    },
    [id, token, navigate],
  );

  useEffect(() => {
    const controller = new AbortController();
    void fetchData(controller.signal);

    return () => controller.abort();
  }, [fetchData]);

  return { genre, isFetching, refetch: fetchData };
}

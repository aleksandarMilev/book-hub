import { HttpStatusCode } from 'axios';
import { useCallback, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '@/features/genre/api/api.js';
import type { GenreDetails, GenreName } from '@/features/genre/types/genre.js';
import { routes } from '@/shared/lib/constants/api.js';
import { errors } from '@/shared/lib/constants/errorMessages.js';
import { IsCanceledError, IsError } from '@/shared/lib/utils/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';
import { HttpError } from '@/shared/types/errors/httpError.js';

export const useAll = () => {
  const { token } = useAuth();
  const navigate = useNavigate();

  const [genres, setGenres] = useState<GenreName[]>([]);
  const [isFetching, setIsFetching] = useState<boolean>(false);

  const fetchData = useCallback(
    async (signal?: AbortSignal) => {
      if (!token) {
        return;
      }

      try {
        setIsFetching(true);
        const data = await api.all(token, signal);
        setGenres(data);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const message = IsError(error) ? error.message : 'Failed to load genres.';
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
};

export const useDetails = (id?: string) => {
  const { token } = useAuth();

  const [genre, setGenre] = useState<GenreDetails | null>(null);
  const [isFetching, setIsFetching] = useState(false);
  const [error, setError] = useState<HttpError | null>(null);

  useEffect(() => {
    if (!id) {
      setError(
        HttpError.with()
          .message(errors.genre.byId)
          .and()
          .name('Genre Error')
          .and()
          .status(HttpStatusCode.NotFound)
          .create(),
      );

      return;
    }

    const controller = new AbortController();
    (async () => {
      try {
        setIsFetching(true);
        const data = await api.details(id, token, controller.signal);
        setGenre(data ?? []);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        setError(error as HttpError);
      } finally {
        setIsFetching(false);
      }
    })();

    return () => controller.abort();
  }, [id, token]);

  return { genre, isFetching, error };
};

export const useSearch = (genres: GenreName[], selectedGenres: GenreName[]) => {
  const [searchTerm, setSearchTerm] = useState<string>('');
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
};

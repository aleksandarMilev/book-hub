import { useCallback, useContext, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '../api/author/authorApi';
import { routes } from '../common/constants/api';
import { UserContext } from '../contexts/user/userContext';
import type { Author } from '../api/author/types/author.type';

export function useGetTopThree() {
  const { token } = useContext(UserContext);
  const [authors, setAuthors] = useState<Author[]>([]);
  const [isFetching, setIsFetching] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!token) {
      return;
    }

    const fetchData = async () => {
      try {
        setIsFetching(true);
        const data = await api.topThree(token);
        setAuthors(data);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to load authors.';
        setError(message);
      } finally {
        setIsFetching(false);
      }
    };

    void fetchData();
  }, [token]);

  return { authors, isFetching, error };
}

export function useNames() {
  const { token } = useContext(UserContext);
  const navigate = useNavigate();
  const [authors, setAuthors] = useState<Author[]>([]);
  const [isFetching, setIsFetching] = useState(false);

  useEffect(() => {
    if (!token) {
      return;
    }

    const fetchData = async () => {
      try {
        setIsFetching(true);
        const data = await api.names(token);
        setAuthors(data);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to load author names.';
        navigate(routes.badRequest, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    };

    void fetchData();
  }, [token, navigate]);

  return { authors, isFetching };
}

export function useSearchAuthors(authors: Author[]) {
  const [searchTerm, setSearchTerm] = useState('');
  const [filteredAuthors, setFilteredAuthors] = useState<Author[]>([]);
  const [showDropdown, setShowDropdown] = useState(false);

  useEffect(() => {
    if (!searchTerm.trim()) {
      setFilteredAuthors([]);
      return;
    }

    const filtered = authors.filter((a) => a.name.toLowerCase().includes(searchTerm.toLowerCase()));
    setFilteredAuthors(filtered);
  }, [searchTerm, authors]);

  const updateSearchTerm = useCallback((term: string) => {
    setSearchTerm(term);
    setShowDropdown(true);
  }, []);

  const selectAuthor = useCallback((name: string) => {
    setSearchTerm(name);
    setShowDropdown(false);
  }, []);

  const showDropdownOnFocus = useCallback(() => setShowDropdown(true), []);

  return {
    searchTerm,
    filteredAuthors,
    showDropdown,
    updateSearchTerm,
    selectAuthor,
    showDropdownOnFocus,
  };
}

export function useGetDetails(id: number) {
  const { token, isAdmin } = useContext(UserContext);
  const navigate = useNavigate();
  const [author, setAuthor] = useState<Author | null>(null);
  const [isFetching, setIsFetching] = useState(false);

  useEffect(() => {
    if (!id || !token) {
      return;
    }

    const fetchData = async () => {
      try {
        setIsFetching(true);
        const data = await api.details(id, token, isAdmin);
        setAuthor(data);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Author not found.';
        navigate(routes.notFound, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    };

    void fetchData();
  }, [id, token, isAdmin, navigate]);

  return { author, isFetching };
}

export function useCreate() {
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const createHandler = useCallback(
    async (authorData: Omit<Author, 'id'>) => {
      const authorToSend: Author = {
        ...authorData,
        penName: authorData.penName || null,
        imageUrl: authorData.imageUrl || null,
        ...(authorData.nationalityId && { nationalityId: authorData.nationalityId }),
      };

      try {
        return await api.create(authorToSend, token);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to create author.';
        navigate(routes.badRequest, { state: { message } });
      }
    },
    [token, navigate],
  );

  return createHandler;
}

export function useEdit() {
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const editHandler = useCallback(
    async (id: number, authorData: Author) => {
      const authorToSend: Author = {
        ...authorData,
        penName: authorData.penName || null,
        imageUrl: authorData.imageUrl || null,
        ...(authorData.nationalityId && { nationalityId: authorData.nationalityId }),
      };

      try {
        return await api.edit(id, authorToSend, token);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to edit author.';
        navigate(routes.badRequest, { state: { message } });
      }
    },
    [token, navigate],
  );

  return editHandler;
}

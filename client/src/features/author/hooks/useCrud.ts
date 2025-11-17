import { HttpStatusCode } from 'axios';
import { useCallback, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '@/features/author/api/api.js';
import type {
  Author,
  AuthorDetails,
  AuthorNames,
  CreateAuthor,
} from '@/features/author/types/author.js';
import { routes } from '@/shared/lib/constants/api.js';
import { errors } from '@/shared/lib/constants/errorMessages.js';
import { IsCanceledError, IsError } from '@/shared/lib/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';
import { useMessage } from '@/shared/stores/message/message.js';
import { HttpError } from '@/shared/types/errors/httpError.js';
import type { IntId } from '@/shared/types/intId.js';

export const useTopThree = () => {
  const [authors, setAuthors] = useState<Author[]>([]);
  const [isFetching, setIsFetching] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const controller = new AbortController();

    const fetchAuthors = async () => {
      try {
        setIsFetching(true);

        const data = await api.topThree(controller.signal);
        if (data) {
          setAuthors(data);
        }
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const message = IsError(error) ? error.message : 'Failed to load authors.';
        setError(message);
      } finally {
        setIsFetching(false);
      }
    };

    void fetchAuthors();
    return () => controller.abort();
  }, []);

  return { authors, isFetching, error };
};

export const useNames = () => {
  const { token } = useAuth();
  const navigate = useNavigate();

  const [authors, setAuthors] = useState<AuthorNames[]>([]);
  const [isFetching, setIsFetching] = useState(false);

  useEffect(() => {
    const controller = new AbortController();
    const fetchData = async () => {
      try {
        setIsFetching(true);

        const data = await api.names(token, controller.signal);
        if (data) {
          setAuthors(data);
        }
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const message = IsError(error) ? error.message : 'Failed to load author names.';
        navigate(routes.badRequest, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    };

    void fetchData();
    return () => controller.abort();
  }, [token, navigate]);

  return { authors, isFetching };
};

export const useSearchNames = (authors: AuthorNames[]) => {
  const [searchTerm, setSearchTerm] = useState('');
  const [filteredAuthors, setFilteredAuthors] = useState<AuthorNames[]>([]);
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
};

export const useDetails = (id: IntId | null, disable = false) => {
  const { token, isAdmin } = useAuth();
  const [author, setAuthor] = useState<AuthorDetails | null>(null);
  const [isFetching, setIsFetching] = useState(false);
  const [error, setError] = useState<HttpError | null>(null);

  useEffect(() => {
    const controller = new AbortController();

    const fetchData = async () => {
      try {
        setIsFetching(true);

        if (disable || !id) {
          setError(
            HttpError.with()
              .message(errors.author.byId)
              .and()
              .name('Author Error')
              .and()
              .status(HttpStatusCode.NotFound)
              .create(),
          );
          return;
        }

        const data = await api.details(id, token, isAdmin, controller.signal);
        setAuthor(data);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        setError(error as HttpError);
      } finally {
        setIsFetching(false);
      }
    };

    void fetchData();

    return () => controller.abort();
  }, [id, token, isAdmin, disable]);

  return { author, isFetching, error };
};

export const useCreate = () => {
  const { token } = useAuth();
  const navigate = useNavigate();

  const createHandler = useCallback(
    async (authorData: CreateAuthor) => {
      const authorToSend: CreateAuthor = {
        ...authorData,
        penName: authorData.penName || '',
        imageUrl: authorData.imageUrl || '',
      };

      try {
        return await api.create(authorToSend, token);
      } catch (error) {
        const message = IsError(error) ? error.message : 'Failed to create author.';
        navigate(routes.badRequest, { state: { message } });

        return undefined;
      }
    },
    [token, navigate],
  );

  return createHandler;
};

export const useEdit = () => {
  const { token } = useAuth();
  const navigate = useNavigate();

  const editHandler = useCallback(
    async (id: number, authorData: CreateAuthor) => {
      const authorToSend: CreateAuthor = {
        ...authorData,
        penName: authorData.penName || '',
        imageUrl: authorData.imageUrl || '',
      };

      try {
        await api.edit(id, authorToSend, token);
      } catch (error) {
        const message = IsError(error) ? error.message : 'Failed to edit author.';
        navigate(routes.badRequest, { state: { message } });
      }
    },
    [token, navigate],
  );

  return editHandler;
};

export const useRemove = (id: IntId | null, disable = false, name?: string) => {
  const { token } = useAuth();
  const navigate = useNavigate();
  const { showMessage } = useMessage();

  const [showModal, setShowModal] = useState(false);
  const toggleModal = useCallback(() => setShowModal((prev) => !prev), []);

  const deleteHandler = useCallback(async () => {
    if (disable || !id) {
      return;
    }

    if (!showModal) {
      toggleModal();
      return;
    }

    const controller = new AbortController();

    try {
      await api.remove(id, token);

      showMessage(`${name || 'This author'} was successfully deleted!`, true);
      navigate(routes.home);
    } catch (error) {
      if (IsCanceledError(error)) {
        return;
      }

      const message = IsError(error) ? error.message : 'Failed to delete author.';
      showMessage(message, false);
    } finally {
      toggleModal();
      controller.abort();
    }
  }, [showModal, id, token, name, navigate, showMessage, toggleModal, disable]);

  return { showModal, toggleModal, deleteHandler };
};

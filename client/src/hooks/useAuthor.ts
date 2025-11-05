import { useCallback, useContext, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '../api/author/authorApi';
import { routes } from '../common/constants/api';
import { UserContext } from '../contexts/user/userContext';
import { useMessage } from '../contexts/message/messageContext';
import type {
  Author,
  AuthorDetails,
  AuthorInput,
  AuthorName,
  UseAuthorApprovalProps,
} from '../api/author/types/author';

export function useTopThree() {
  const { token } = useContext(UserContext);

  const [authors, setAuthors] = useState<AuthorDetails[]>([]);
  const [isFetching, setIsFetching] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const controller = new AbortController();
    const fetchAuthors = async () => {
      try {
        setIsFetching(true);

        const data = await api.topThree(token, controller.signal);
        if (data) {
          setAuthors(data);
        }
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to load authors.';
        setError(message);
      } finally {
        setIsFetching(false);
      }
    };

    void fetchAuthors();
    return () => controller.abort();
  }, [token]);

  return { authors, isFetching, error };
}

export function useNames() {
  const navigate = useNavigate();
  const { token } = useContext(UserContext);

  const [authors, setAuthors] = useState<AuthorName[]>([]);
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
        const message = error instanceof Error ? error.message : 'Failed to load author names.';
        navigate(routes.badRequest, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    };

    void fetchData();
    return () => controller.abort();
  }, [token, navigate]);

  return { authors, isFetching };
}

export function useSearchNames(authors: AuthorName[]) {
  const [searchTerm, setSearchTerm] = useState('');
  const [filteredAuthors, setFilteredAuthors] = useState<AuthorName[]>([]);
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

export function useDetails(id: number) {
  const navigate = useNavigate();
  const { token, isAdmin } = useContext(UserContext);

  const [author, setAuthor] = useState<AuthorDetails | null>(null);
  const [isFetching, setIsFetching] = useState(false);

  useEffect(() => {
    const controller = new AbortController();
    const fetchData = async () => {
      try {
        setIsFetching(true);

        const data = await api.details(id, token, isAdmin, controller.signal);
        if (!data) {
          return;
        }

        setAuthor(data);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Author not found.';
        navigate(routes.notFound, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    };

    void fetchData();
    return () => controller.abort();
  }, [id, token, isAdmin, navigate]);

  return { author, isFetching };
}

export function useCreate() {
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const createHandler = useCallback(
    async (authorData: AuthorInput) => {
      const authorToSend: AuthorInput = {
        ...authorData,
        penName: authorData.penName || null,
        imageUrl: authorData.imageUrl || null,
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
    async (id: number, authorData: AuthorInput) => {
      const authorToSend: AuthorInput = {
        ...authorData,
        penName: authorData.penName || null,
        imageUrl: authorData.imageUrl || null,
      };

      try {
        await api.edit(id, authorToSend, token);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to edit author.';
        navigate(routes.badRequest, { state: { message } });
      }
    },
    [token, navigate],
  );

  return editHandler;
}

export const useRemove = (id: number, token: string, name?: string) => {
  const navigate = useNavigate();
  const { showMessage } = useMessage();

  const [showModal, setShowModal] = useState(false);

  const toggleModal = useCallback(() => setShowModal((prev) => !prev), []);

  const deleteHandler = useCallback(async () => {
    if (showModal) {
      try {
        await api.remove(id, token);

        showMessage(`${name || 'This author'} was successfully deleted!`, true);
        navigate(routes.home);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to delete author.';
        showMessage(message, false);
      } finally {
        toggleModal();
      }
    } else {
      toggleModal();
    }
  }, [showModal, id, token, name, navigate, showMessage, toggleModal]);

  return { showModal, toggleModal, deleteHandler };
};

export function useAuthorApproval({
  authorId,
  authorName,
  token,
  onSuccess,
}: UseAuthorApprovalProps) {
  const navigate = useNavigate();

  const approveHandler = useCallback(async () => {
    try {
      await api.approve(authorId, token);

      onSuccess(`${authorName} was successfully approved!`, true);
    } catch (error: unknown) {
      const message = error instanceof Error ? error.message : 'Approval failed.';
      onSuccess(message, false);
    }
  }, [authorId, token, authorName, onSuccess]);

  const rejectHandler = useCallback(async () => {
    try {
      await api.reject(authorId, token);

      onSuccess(`${authorName} was successfully rejected!`, true);
      navigate(routes.home);
    } catch (error: unknown) {
      const message = error instanceof Error ? error.message : 'Rejection failed.';
      onSuccess(message, false);
    }
  }, [authorId, token, authorName, onSuccess, navigate]);

  return { approveHandler, rejectHandler };
}

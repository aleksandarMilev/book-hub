import { useContext, useEffect, useState, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '../api/article/articleApi';
import { routes } from '../common/constants/api';
import { UserContext } from '../contexts/user/userContext';
import { useMessage } from '../contexts/message/messageContext';
import type { Article, ArticleInput } from '../api/article/types/article';

export function useDetails(id: number) {
  const { token } = useContext(UserContext);

  const [article, setArticle] = useState<Article | null>(null);
  const [isFetching, setIsFetching] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const controller = new AbortController();
    (async () => {
      try {
        setIsFetching(true);
        setError(null);

        const data = await api.details(id, token, controller.signal);
        setArticle(data);
      } catch (error) {
        if (error instanceof DOMException && error.name === 'AbortError') {
          return;
        }

        const message = error instanceof Error ? error.message : 'Article not found.';
        setError(message);
      } finally {
        setIsFetching(false);
      }
    })();

    return () => controller.abort();
  }, [id, token]);

  return { article, isFetching, error };
}

export function useCreate() {
  const navigate = useNavigate();
  const { token } = useContext(UserContext);

  return useCallback(
    async (articleData: ArticleInput) => {
      const articleToSend: ArticleInput = {
        ...articleData,
        imageUrl: articleData.imageUrl || null,
      };
      try {
        return await api.create(articleToSend, token);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to create article.';
        navigate(routes.badRequest, { state: { message } });

        throw error;
      }
    },
    [token, navigate],
  );
}

export function useEdit() {
  const navigate = useNavigate();
  const { token } = useContext(UserContext);

  return useCallback(
    async (id: number, articleData: ArticleInput) => {
      const articleToSend: ArticleInput = {
        ...articleData,
        imageUrl: articleData.imageUrl || null,
      };
      try {
        const success = await api.edit(id, articleToSend, token);
        if (!success) {
          throw new Error('Failed to edit article.');
        }

        return true;
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to edit article.';
        navigate(routes.badRequest, { state: { message } });
        throw error;
      }
    },
    [token, navigate],
  );
}

export const useRemove = (id: number, title?: string) => {
  const navigate = useNavigate();
  const { showMessage } = useMessage();
  const { token } = useContext(UserContext);

  const [showModal, setShowModal] = useState(false);
  const toggleModal = useCallback(() => setShowModal((prev) => !prev), []);

  const deleteHandler = useCallback(async () => {
    if (!showModal) {
      toggleModal();
      return;
    }

    const controller = new AbortController();
    try {
      await api.remove(id, token, controller.signal);

      showMessage(`${title || 'This article'} was successfully deleted!`, true);
      navigate(routes.home);
    } catch (error: unknown) {
      if (error instanceof DOMException && error.name === 'AbortError') {
        return;
      }

      const message = error instanceof Error ? error.message : 'Failed to delete article.';
      showMessage(message, false);
    } finally {
      toggleModal();
    }

    return () => controller.abort();
  }, [showModal, id, token, title, navigate, showMessage, toggleModal]);

  return { showModal, toggleModal, deleteHandler };
};

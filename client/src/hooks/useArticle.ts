import { useContext, useEffect, useState, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { format } from 'date-fns';

import * as api from '../api/article/articleApi';
import { routes } from '../common/constants/api';
import { UserContext } from '../contexts/user/userContext';
import type { Article, ArticleInput } from '../api/article/types/article.type';
import { useMessage } from '../contexts/message/messageContext';

export function useDetails(id: string) {
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const [article, setArticle] = useState<Article | null>(null);
  const [isFetching, setIsFetching] = useState<boolean>(false);

  useEffect(() => {
    const controller = new AbortController();
    const fetchArticle = async () => {
      try {
        setIsFetching(true);

        const data = await api.details(id, token, controller.signal);
        if (!data) {
          return;
        }

        const formattedArticle: Article = {
          ...data,
          ...(data.createdOn && {
            createdOn: format(new Date(data.createdOn), 'yyyy-MM-dd'),
          }),
        };

        setArticle(formattedArticle);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Article not found.';
        navigate(routes.notFound, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    };

    void fetchArticle();

    return () => controller.abort();
  }, [id, token, navigate]);

  return { article, isFetching };
}

export function useCreate() {
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const createHandler = useCallback(
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
    async (id: number, articleData: ArticleInput) => {
      const articleToSend: ArticleInput = {
        ...articleData,
        imageUrl: articleData.imageUrl || null,
      };

      try {
        await api.edit(id, articleToSend, token);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to edit article.';
        navigate(routes.badRequest, { state: { message } });
      }
    },
    [token, navigate],
  );

  return editHandler;
}

export const useRemove = (id: string, token: string, title?: string) => {
  const [showModal, setShowModal] = useState(false);
  const navigate = useNavigate();
  const { showMessage } = useMessage();

  const toggleModal = useCallback(() => setShowModal((prev) => !prev), []);

  const deleteHandler = useCallback(async () => {
    if (showModal) {
      try {
        await api.remove(id, token);

        showMessage(`${title || 'This article'} was successfully deleted!`, true);
        navigate(routes.home);
      } catch (error: unknown) {
        const message = error instanceof Error ? error.message : 'Failed to delete article.';
        showMessage(message, false);
      } finally {
        toggleModal();
      }
    } else {
      toggleModal();
    }
  }, [showModal, id, token, title, navigate, showMessage, toggleModal]);

  return { showModal, toggleModal, deleteHandler };
};

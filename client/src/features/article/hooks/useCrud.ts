import { HttpStatusCode } from 'axios';
import { useCallback, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '@/features/article/api/api';
import type { ArticleDetails, CreateArticle } from '@/features/article/types/article';
import { routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';
import { IsCanceledError, IsError } from '@/shared/lib/utils';
import { useAuth } from '@/shared/stores/auth/auth';
import { useMessage } from '@/shared/stores/message/message';
import { HttpError } from '@/shared/types/errors/httpError';
import type { IntId } from '@/shared/types/intId';

export const useDetails = (id: IntId | null, disable = false) => {
  const { token } = useAuth();

  const [article, setArticle] = useState<ArticleDetails | null>(null);
  const [isFetching, setIsFetching] = useState(false);
  const [error, setError] = useState<HttpError | null>(null);

  useEffect(() => {
    if (disable || !id) {
      setError(
        HttpError.with()
          .message(errors.article.byId)
          .and()
          .name('Article Error')
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
        setArticle(data);
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
  }, [id, token, disable]);

  return { article, isFetching, error };
};

export const useCreate = () => {
  const { token } = useAuth();
  const navigate = useNavigate();

  return useCallback(
    async (data: CreateArticle) => {
      const articleToSend: CreateArticle = {
        ...data,
        imageUrl: data.imageUrl || null,
      };

      try {
        return await api.create(articleToSend, token);
      } catch (error) {
        const message = IsError(error) ? error.message : errors.article.create;
        navigate(routes.badRequest, { state: { message } });

        throw error;
      }
    },
    [token, navigate],
  );
};

export const useEdit = () => {
  const { token } = useAuth();
  const navigate = useNavigate();

  return useCallback(
    async (id: number, data: CreateArticle) => {
      const articleToSend: CreateArticle = {
        ...data,
        imageUrl: data.imageUrl || null,
      };

      try {
        return await api.edit(id, articleToSend, token);
      } catch (error) {
        const message = IsError(error) ? error.message : errors.article.edit;
        navigate(routes.badRequest, { state: { message } });

        throw error;
      }
    },
    [token, navigate],
  );
};

export const useRemove = (id: IntId | null, disable = false, title?: string) => {
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
      await api.remove(id, token, controller.signal);

      showMessage(`${title || 'This article'} was successfully deleted!`, true);
      navigate(routes.home);
    } catch (error) {
      if (IsCanceledError(error)) {
        return;
      }

      const message = IsError(error) ? error.message : errors.article.delete;
      showMessage(message, false);
    } finally {
      toggleModal();
      controller.abort();
    }
  }, [showModal, id, token, title, navigate, showMessage, toggleModal, disable]);

  return { showModal, toggleModal, deleteHandler };
};

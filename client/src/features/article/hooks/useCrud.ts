import { HttpStatusCode } from 'axios';
import { useCallback, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '@/features/article/api/api.js';
import type { ArticleDetails, CreateArticle } from '@/features/article/types/article.js';
import { routes } from '@/shared/lib/constants/api.js';
import { errors } from '@/shared/lib/constants/errorMessages.js';
import { IsCanceledError, IsError } from '@/shared/lib/utils/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';
import { useMessage } from '@/shared/stores/message/message.js';
import { HttpError } from '@/shared/types/errors/httpError.js';

export const useDetails = (id?: string, isEditMode = false) => {
  const { token } = useAuth();
  const [article, setArticle] = useState<ArticleDetails | null>(null);
  const [isFetching, setIsFetching] = useState(false);
  const [error, setError] = useState<HttpError | null>(null);

  useEffect(() => {
    if (!id) {
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
        const data = isEditMode
          ? await api.detailsForEdit(id, token, controller.signal)
          : await api.details(id, token, controller.signal);

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
  }, [id, token, isEditMode]);

  return { article, isFetching, error };
};

export const useCreate = () => {
  const { token } = useAuth();
  const navigate = useNavigate();

  return useCallback(
    async (data: CreateArticle) => {
      try {
        return await api.create(data, token);
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
    async (id: string, data: CreateArticle) => {
      try {
        return await api.edit(id, data, token);
      } catch (error) {
        const message = IsError(error) ? error.message : errors.article.edit;
        navigate(routes.badRequest, { state: { message } });

        throw error;
      }
    },
    [token, navigate],
  );
};

export const useRemove = (id?: string, title?: string) => {
  const { token } = useAuth();
  const navigate = useNavigate();
  const { showMessage } = useMessage();
  const [showModal, setShowModal] = useState(false);
  const toggleModal = useCallback(() => setShowModal((prev) => !prev), []);

  const deleteHandler = useCallback(async () => {
    if (!id) {
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
  }, [showModal, id, token, title, navigate, showMessage, toggleModal]);

  return { showModal, toggleModal, deleteHandler };
};

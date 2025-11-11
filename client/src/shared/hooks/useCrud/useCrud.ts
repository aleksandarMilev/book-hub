import { HttpStatusCode } from 'axios';
import { useCallback, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import type { CrudHookOptions } from '@/shared/hooks/useCrud/types/crudHookOptions';
import { routes } from '@/shared/lib/constants/api';
import { IsCanceledError, IsError } from '@/shared/lib/utils';
import { useAuth } from '@/shared/stores/auth/auth';
import { useMessage } from '@/shared/stores/message/message';
import { HttpError } from '@/shared/types/errors/httpError';
import type { IntId } from '@/shared/types/intId';

export function createCrudHooks<TAll, TDetails, TCreate>({
  api,
  resourceName,
  errors,
}: CrudHookOptions<TAll, TDetails, TCreate>) {
  function useAll(disable = false) {
    const { token } = useAuth();

    const [data, setData] = useState<TAll | null>(null);
    const [isFetching, setIsFetching] = useState(false);
    const [error, setError] = useState<HttpError | null>(null);

    useEffect(() => {
      if (disable) {
        return;
      }

      const all = api.all;
      if (!all) {
        throw new Error('all() is not implemented!');
      }

      const controller = new AbortController();
      (async () => {
        try {
          setIsFetching(true);
          const fetched = await all(token, controller.signal);
          setData(fetched);
        } catch (error) {
          if (!IsCanceledError(error)) {
            setError(error as HttpError);
          }
        } finally {
          setIsFetching(false);
        }
      })();

      return () => controller.abort();
    }, [token, disable, data, isFetching]);

    return { data, isFetching, error };
  }

  function useDetails(id: IntId | null, disable = false) {
    const { token } = useAuth();

    const [data, setData] = useState<TDetails | null>(null);
    const [isFetching, setIsFetching] = useState(false);
    const [error, setError] = useState<HttpError | null>(null);

    useEffect(() => {
      if (disable || !id) {
        setError(
          HttpError.with()
            .message(errors.byId ?? `${resourceName} not found`)
            .and()
            .name(`${resourceName} Error`)
            .and()
            .status(HttpStatusCode.NotFound)
            .create(),
        );

        return;
      }

      const details = api.details;
      if (!details) {
        throw new Error('details() is not implemented!');
      }

      const controller = new AbortController();
      (async () => {
        try {
          setIsFetching(true);
          const fetched = await details(id, token, controller.signal);
          setData(fetched);
        } catch (error) {
          if (!IsCanceledError(error)) {
            setError(error as HttpError);
          }
        } finally {
          setIsFetching(false);
        }
      })();

      return () => controller.abort();
    }, [id, token, disable, data, isFetching, error]);

    return { data, isFetching, error };
  }

  function useCreate() {
    const { token } = useAuth();
    const navigate = useNavigate();

    const create = api.create;
    if (!create) {
      throw new Error('create() is not implemented!');
    }

    return useCallback(
      async (data: TCreate) => {
        try {
          return await create(data, token);
        } catch (error) {
          const message = IsError(error) ? error.message : errors.create;
          navigate(routes.badRequest, { state: { message } });

          throw error;
        }
      },
      [token, navigate, create],
    );
  }

  function useEdit() {
    const { token } = useAuth();
    const navigate = useNavigate();

    const edit = api.edit;
    if (!edit) {
      throw new Error('edit() is not implemented!');
    }

    return useCallback(
      async (id: number, data: TCreate) => {
        try {
          return await edit(id, data, token);
        } catch (error) {
          const message = IsError(error) ? error.message : errors.edit;
          navigate(routes.badRequest, { state: { message } });

          throw error;
        }
      },
      [token, navigate, edit],
    );
  }

  function useRemove(id: IntId | null, disable = false, title?: string) {
    const { token } = useAuth();
    const navigate = useNavigate();
    const { showMessage } = useMessage();

    const [showModal, setShowModal] = useState(false);
    const toggleModal = useCallback(() => setShowModal((prev) => !prev), []);

    const deleteHandler = useCallback(async () => {
      if (disable || !id) {
        return;
      }

      const remove = api.remove;
      if (!remove) {
        throw new Error('remove() is not implemented!');
      }

      if (!showModal) {
        toggleModal();
        return;
      }

      const controller = new AbortController();
      try {
        await remove(id, token, controller.signal);

        showMessage(`${title || resourceName} was successfully deleted!`, true);
        navigate(routes.home);
      } catch (error) {
        if (!IsCanceledError(error)) {
          const message = IsError(error) ? error.message : errors.delete;
          showMessage(message ?? 'Delete failed!', false);
        }
      } finally {
        toggleModal();
        controller.abort();
      }
    }, [id, disable, showModal, token, title, navigate, showMessage, toggleModal]);

    return { showModal, toggleModal, deleteHandler };
  }

  function usePatch() {
    const { token } = useAuth();
    const navigate = useNavigate();

    const patch = api.patch;
    if (!patch) {
      throw new Error('patch() is not implemented!');
    }

    return useCallback(
      async (id: number, data: TCreate) => {
        try {
          return await patch(id, data, token);
        } catch (error) {
          const message = IsError(error) ? error.message : errors.edit;
          navigate(routes.badRequest, { state: { message } });

          throw error;
        }
      },
      [token, navigate, patch],
    );
  }

  return { useAll, useDetails, useCreate, useEdit, useRemove, usePatch };
}

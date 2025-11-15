import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import { routes } from '@/shared/lib/constants/api.js';
import { IsCanceledError, IsError } from '@/shared/lib/utils.js';

export function usePublicPost<TRequest, TResponse>(
  requestFunction: (data: TRequest, signal?: AbortSignal) => Promise<TResponse>,
  errorMessage: string,
  onSuccess?: (response: TResponse) => Promise<void> | void,
) {
  const navigate = useNavigate();

  return useCallback(
    async (data: TRequest) => {
      const controller = new AbortController();
      try {
        const result = await requestFunction(data, controller.signal);
        await onSuccess?.(result);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const message = IsError(error) ? error.message : errorMessage;
        navigate(routes.badRequest, { state: { message } });
      }
      return () => controller.abort();
    },
    [navigate, requestFunction, errorMessage, onSuccess],
  );
}

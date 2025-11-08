import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import { routes } from '@/shared/lib/constants/api';
import { IsDomAbortError, IsError } from '@/shared/lib/utils';

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
        if (IsDomAbortError(error)) {
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

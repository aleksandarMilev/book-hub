import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '@/features/author/api/api.js';
import { routes } from '@/shared/lib/constants/api.js';
import { IsError } from '@/shared/lib/utils/utils.js';

export const useAuthorApproval = ({
  authorId,
  authorName,
  token,
  onSuccess,
}: {
  authorId: string;
  authorName: string;
  token: string;
  onSuccess: (message: string, success?: boolean) => void;
}) => {
  const navigate = useNavigate();
  const approveHandler = useCallback(async () => {
    try {
      await api.approve(authorId, token);

      onSuccess(`${authorName} was successfully approved!`, true);
    } catch (error) {
      const message = IsError(error) ? error.message : 'Approval failed.';
      onSuccess(message, false);
    }
  }, [authorId, token, authorName, onSuccess]);

  const rejectHandler = useCallback(async () => {
    try {
      await api.reject(authorId, token);

      onSuccess(`${authorName} was successfully rejected!`, true);
      navigate(routes.home);
    } catch (error) {
      const message = IsError(error) ? error.message : 'Rejection failed.';
      onSuccess(message, false);
    }
  }, [authorId, token, authorName, onSuccess, navigate]);

  return { approveHandler, rejectHandler };
};

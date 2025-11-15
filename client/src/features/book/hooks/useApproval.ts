import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import * as api from '@/features/book/api/api';
import { routes } from '@/shared/lib/constants/api';

export function useApproval(
  id: number,
  token: string,
  showMessage: (message: string, isSuccess: boolean) => void,
) {
  const navigate = useNavigate();
  const approveHandler = useCallback(async () => {
    try {
      await api.approve(id, token);

      showMessage('You have successfully approved the book!', true);
      return true;
    } catch {
      showMessage('Error approving the book. Please try again!', false);
      return false;
    }
  }, [id, token, showMessage]);

  const rejectHandler = useCallback(async () => {
    try {
      await api.reject(id, token);

      showMessage('You have successfully rejected the book!', true);
      navigate(routes.home);

      return true;
    } catch {
      showMessage('Error rejecting the book. Please try again!', false);
      return false;
    }
  }, [id, token, showMessage, navigate]);

  return { approveHandler, rejectHandler };
}

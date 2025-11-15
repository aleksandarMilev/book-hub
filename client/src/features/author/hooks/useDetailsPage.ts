import { useNavigate, useParams } from 'react-router-dom';

import { useDetails, useRemove } from '@/features/author/hooks/useCrud.js';
import { toIntId } from '@/shared/lib/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';
import { useMessage } from '@/shared/stores/message/message.js';

export const useDetailsPage = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const parsedId = toIntId(id);
  const disable = !parsedId;

  const { showMessage } = useMessage();
  const { token, isAdmin, userId } = useAuth();
  const { author, isFetching, error } = useDetails(parsedId, disable);
  const { showModal, toggleModal, deleteHandler } = useRemove(parsedId, disable, author?.name);

  return {
    parsedId,
    token,
    isAdmin,
    userId,
    author,
    isFetching,
    error,
    showModal,
    toggleModal,
    deleteHandler,
    navigate,
    showMessage,
  };
};

import { useState, type FC } from 'react';
import type { ApproveRejectButtonsProps } from './types/approveRejectButtonsProps';
import { useNavigate } from 'react-router-dom';
import * as api from '../../../../api/author/authorApi';
import { routes } from '../../../../common/constants/api';

const ApproveRejectButtons: FC<ApproveRejectButtonsProps> = ({
  authorId,
  authorName,
  initialIsApproved,
  token,
  onSuccess,
}) => {
  const navigate = useNavigate();
  const [isApproved, setIsApproved] = useState(initialIsApproved);

  const approveHandler = async () => {
    try {
      await api.approve(authorId, token);

      setIsApproved(true);
      onSuccess(`${authorName} was successfully approved!`);
    } catch (error: any) {
      onSuccess(error.message ?? 'Approval failed.', false);
    }
  };

  const rejectHandler = async () => {
    try {
      await api.reject(authorId, token);

      onSuccess(`${authorName} was successfully rejected!`);
      navigate(routes.home);
    } catch (error: any) {
      onSuccess(error.message ?? 'Rejection failed.', false);
    }
  };

  if (isApproved) {
    return <p className="text-success">This author has been approved.</p>;
  }

  return (
    <div className="author-actions">
      <button className="btn btn-success me-2" onClick={approveHandler}>
        Approve
      </button>
      <button className="btn btn-danger" onClick={rejectHandler}>
        Reject
      </button>
    </div>
  );
};

export default ApproveRejectButtons;

import { useState, type FC } from 'react';

import type { ApproveRejectButtonsProps } from '../../../../api/author/types/author';
import { useAuthorApproval } from '../../../../hooks/useAuthor';


const ApproveRejectButtons: FC<ApproveRejectButtonsProps> = ({
  authorId,
  authorName,
  initialIsApproved,
  token,
  onSuccess,
}) => {
  const [isApproved, setIsApproved] = useState(initialIsApproved);
  const { approveHandler, rejectHandler } = useAuthorApproval({
    authorId,
    authorName,
    token,
    onSuccess: (message, success = true) => {
      onSuccess(message, success);
      if (success && message.includes('approved')) {
        setIsApproved(true);
      }
    },
  });

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

import { type FC, useState } from 'react';

import { useAuthorApproval } from '@/features/author/hooks/useApproval.js';

const ApproveRejectButtons: FC<{
  authorId: number;
  authorName: string;
  initialIsApproved: boolean;
  token: string;
  onSuccess: (message: string, success?: boolean) => void;
}> = ({ authorId, authorName, initialIsApproved, token, onSuccess }) => {
  const [isApproved, setIsApproved] = useState(initialIsApproved);
  const { approveHandler, rejectHandler } = useAuthorApproval({
    authorId,
    authorName,
    token,
    onSuccess: (message: string, success = true) => {
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

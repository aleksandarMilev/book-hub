import { useState, type FC } from 'react';

import { useApproval } from '../../../../../hooks/useBook';

export const ApproveRejectButtons: FC<{
  id: number;
  initialIsApproved: boolean;
  token: string;
  showMessage: (message: string, success?: boolean) => void;
}> = ({ id, initialIsApproved, token, showMessage }) => {
  const [isApproved, setIsApproved] = useState(initialIsApproved);
  const { approveHandler, rejectHandler } = useApproval({ id, token, showMessage });

  const handleApprove = async () => {
    const success = await approveHandler();
    if (success) {
      setIsApproved(true);
    }
  };

  return !isApproved ? (
    <div className="d-flex gap-2">
      <button className="btn btn-success d-flex align-items-center gap-2" onClick={handleApprove}>
        Approve
      </button>
      <button className="btn btn-danger d-flex align-items-center gap-2" onClick={rejectHandler}>
        Reject
      </button>
    </div>
  ) : (
    <p className="text-success">This book has been approved.</p>
  );
};

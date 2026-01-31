import './ApproveRejectButtons.css';

import { type FC, useState } from 'react';

import { useApproval } from '@/features/book/hooks/useApproval';

type Props = {
  id: string;
  initialIsApproved: boolean;
  token: string;
  showMessage: (message: string, success?: boolean) => void;
};

export const ApproveRejectButtons: FC<Props> = ({ id, initialIsApproved, token, showMessage }) => {
  const [isApproved, setIsApproved] = useState(initialIsApproved);
  const { approveHandler, rejectHandler } = useApproval(id, token, showMessage);

  const handleApprove = async () => {
    const success = await approveHandler();
    if (success) {
      setIsApproved(true);
    }
  };

  if (isApproved) {
    return <p className="book-approve-status text-success">This book has been approved.</p>;
  }

  return (
    <div className="book-approve-buttons">
      <button className="btn btn-success book-approve-button" type="button" onClick={handleApprove}>
        Approve
      </button>
      <button className="btn btn-danger book-reject-button" type="button" onClick={rejectHandler}>
        Reject
      </button>
    </div>
  );
};



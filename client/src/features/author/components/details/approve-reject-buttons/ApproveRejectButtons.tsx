import { type FC, useState } from 'react';
import { useTranslation } from 'react-i18next';

import { useAuthorApproval } from '@/features/author/hooks/useApproval.js';

type Props = {
  authorId: string;
  authorName: string;
  initialIsApproved: boolean;
  token: string;
  onSuccess: (message: string, success?: boolean) => void;
};

const ApproveRejectButtons: FC<Props> = ({
  authorId,
  authorName,
  initialIsApproved,
  token,
  onSuccess,
}) => {
  const { t } = useTranslation('authors');
  const [isApproved, setIsApproved] = useState(initialIsApproved);
  const { approveHandler, rejectHandler } = useAuthorApproval({
    authorId,
    authorName,
    token,
    onSuccess: (message, success = true) => {
      onSuccess(message, success);
      if (success) {
        setIsApproved(true);
      }
    },
  });

  if (isApproved) {
    return <p className="text-success">{t('details.approval.alreadyApproved')}</p>;
  }

  return (
    <div className="author-actions">
      <button
        className="btn btn-success me-2"
        type="button"
        onClick={approveHandler}
        aria-label={t('details.approval.approve')}
      >
        {t('details.approval.approve')}
      </button>
      <button
        className="btn btn-danger"
        type="button"
        onClick={rejectHandler}
        aria-label={t('details.approval.reject')}
      >
        {t('details.approval.reject')}
      </button>
    </div>
  );
};

export default ApproveRejectButtons;

import './HomePageError.css';

import type { FC } from 'react';
import { useTranslation } from 'react-i18next';
import { FaExclamationTriangle } from 'react-icons/fa';

const HomePageError: FC<{
  message: string;
  onRetry?: () => void;
}> = ({ message, onRetry }) => {
  const { t } = useTranslation('common');

  return (
    <div className="error-block-wrapper">
      <div className="error-block">
        <FaExclamationTriangle className="error-icon" />
        <p className="error-message">{message}</p>
        {onRetry && (
          <button className="error-retry-btn" onClick={onRetry}>
            {t('errors.homePage.tryAgain')}
          </button>
        )}
      </div>
    </div>
  );
};

export default HomePageError;

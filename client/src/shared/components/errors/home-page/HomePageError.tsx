import './HomePageError.css';

import type { FC } from 'react';
import { FaExclamationTriangle } from 'react-icons/fa';

const HomePageError: FC<{
  message: string;
  onRetry?: () => void;
}> = ({ message, onRetry }) => {
  return (
    <div className="error-block-wrapper">
      <div className="error-block">
        <FaExclamationTriangle className="error-icon" />
        <p className="error-message">{message}</p>
        {onRetry && (
          <button className="error-retry-btn" onClick={onRetry}>
            Try Again
          </button>
        )}
      </div>
    </div>
  );
};

export default HomePageError;

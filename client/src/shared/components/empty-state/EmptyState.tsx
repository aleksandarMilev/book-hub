import './EmptyState.css';

import type { FC, ReactNode } from 'react';

const EmptyState: FC<{
  title?: string;
  message: string;
  icon: ReactNode;
  actionLabel?: string;
  onAction?: () => void;
}> = ({ title, message, icon, actionLabel, onAction }) => {
  return (
    <div className="empty-state-wrapper">
      <div className="empty-state-card">
        <div className="empty-state-icon">{icon}</div>
        {title && <h3 className="empty-state-title">{title}</h3>}
        <p className="empty-state-message">{message}</p>
        {actionLabel && onAction && (
          <button className="empty-state-button" onClick={onAction}>
            {actionLabel}
          </button>
        )}
      </div>
    </div>
  );
};

export default EmptyState;

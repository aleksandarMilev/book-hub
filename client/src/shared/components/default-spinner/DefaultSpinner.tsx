import './DefaultSpinner.css';

import type { FC } from 'react';

const DefaultSpinner: FC = () => {
  return (
    <div className="spinner-wrapper">
      <div className="bookhub-spinner"></div>
      <p className="spinner-text">Loading...</p>
    </div>
  );
};

export default DefaultSpinner;

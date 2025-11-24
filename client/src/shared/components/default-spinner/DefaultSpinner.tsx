import './DefaultSpinner.css';

import type { FC } from 'react';
import { useTranslation } from 'react-i18next';

const DefaultSpinner: FC = () => {
  const { t } = useTranslation('common');

  return (
    <div className="spinner-wrapper">
      <div className="bookhub-spinner"></div>
      <p className="spinner-text">{t('spinner.loading')}</p>
    </div>
  );
};

export default DefaultSpinner;

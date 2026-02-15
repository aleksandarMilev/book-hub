import './StorageNoticePage.css';

import { MDBBtn } from 'mdb-react-ui-kit';
import type { FC } from 'react';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Link } from 'react-router-dom';

import { routes } from '@/shared/lib/constants/api';

const STORAGE_KEY = 'bookhub_storage_notice_dismissed_v1';

const StorageNotice: FC = () => {
  const { t } = useTranslation('legal');
  const [visible, setVisible] = useState(false);

  useEffect(() => {
    try {
      const dismissed = localStorage.getItem(STORAGE_KEY) === '1';
      setVisible(!dismissed);
    } catch {
      setVisible(true);
    }
  }, []);

  if (!visible) {
    return null;
  }

  const dismiss = () => {
    setVisible(false);
    try {
      localStorage.setItem(STORAGE_KEY, '1');
    } catch {
      // ignore
    }
  };

  return (
    <div className="legal-notice-wrap" role="dialog" aria-live="polite">
      <div className="legal-notice-card">
        <div className="legal-notice-text">
          {t('notice.text')}{' '}
          <Link to={routes.legal.cookies} className="legal-notice-link">
            {t('notice.learnMore')}
          </Link>
          .
        </div>
        <div className="legal-notice-actions">
          <MDBBtn color="dark" size="sm" className="legal-notice-btn" onClick={dismiss}>
            {t('notice.ok')}
          </MDBBtn>
        </div>
      </div>
    </div>
  );
};

export default StorageNotice;

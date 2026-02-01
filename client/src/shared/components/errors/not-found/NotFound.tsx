import type { FC } from 'react';
import { useTranslation } from 'react-i18next';
import { Link, useLocation } from 'react-router-dom';

import type { ErrorComponentLocationState } from '@/shared/components/errors/types/errorComponentLocationState';
import { routes } from '@/shared/lib/constants/api';

import image from './not-found.webp';

const NotFound: FC = () => {
  const { t } = useTranslation('common');
  const location = useLocation();
  const state = location.state as ErrorComponentLocationState | null;
  const message = state?.message || t('errors.notFound.defaultMessage');

  return (
    <div className="d-flex align-items-center justify-content-center vh-100">
      <div className="text-center">
        <img
          src={image}
          alt={t('errors.notFound.imageAlt')}
          className="img-fluid mb-4"
          style={{ maxWidth: '300px' }}
        />
        <p className="fs-3 text-danger mb-3">{t('errors.notFound.oops')}</p>
        <p className="lead">{message}</p>
        <Link to={routes.home} className="btn btn-primary">
          {t('buttons.goHome')}
        </Link>
      </div>
    </div>
  );
};

export default NotFound;



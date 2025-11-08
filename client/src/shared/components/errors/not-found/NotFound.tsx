import type { FC } from 'react';
import { Link, useLocation } from 'react-router-dom';

import type { ErrorComponentLocationState } from '@/shared/components/errors/types/errorComponentLocationState';
import { routes } from '@/shared/lib/constants/api';

import image from './not-found.webp';

const NotFound: FC = () => {
  const location = useLocation();
  const state = location.state as ErrorComponentLocationState | null;
  const message =
    state?.message ||
    "We couldn't find what you're looking for. Please check the URL or try again later.";

  return (
    <div className="d-flex align-items-center justify-content-center vh-100">
      <div className="text-center">
        <img
          src={image}
          alt="Page not found illustration"
          className="img-fluid mb-4"
          style={{ maxWidth: '300px' }}
        />
        <p className="fs-3 text-danger mb-3">Oops!</p>
        <p className="lead">{message}</p>
        <Link to={routes.home} className="btn btn-primary">
          Go Home
        </Link>
      </div>
    </div>
  );
};

export default NotFound;

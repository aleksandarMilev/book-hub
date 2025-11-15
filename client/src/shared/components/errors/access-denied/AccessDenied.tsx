import type { FC } from 'react';
import { Link, useLocation } from 'react-router-dom';

import type { ErrorComponentLocationState } from '@/shared/components/errors/types/errorComponentLocationState.js';
import { routes } from '@/shared/lib/constants/api.js';

import image from './forbidden.png';

const AccessDenied: FC = () => {
  const location = useLocation();
  const state = location.state as ErrorComponentLocationState | null;
  const message = state?.message || 'Sorry, you cannot access this resource.';

  return (
    <div className="d-flex align-items-center justify-content-center vh-100">
      <div className="text-center">
        <img
          src={image}
          alt="Access denied illustration"
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

export default AccessDenied;

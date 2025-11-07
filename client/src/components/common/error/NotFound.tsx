import type { FC } from 'react';
import { Link, useLocation } from 'react-router-dom';

import type { ErrorComponentLocationState } from './types/errorComponentLocationState';

import image from '../../../assets/images/not-found.webp';
import { routes } from '../../../common/constants/api';


const NotFound: FC = () => {
  const location = useLocation();
  const state = location.state as ErrorComponentLocationState | null;
  const message =
    state?.message ||
    'We couldn’t find what you’re looking for. Please check the URL or try again later.';

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

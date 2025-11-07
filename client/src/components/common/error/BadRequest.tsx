import type { FC } from 'react';
import { Link, useLocation } from 'react-router-dom';

import type { ErrorComponentLocationState } from './types/errorComponentLocationState';

import image from '../../../assets/images/bad-request.png';
import { routes } from '../../../common/constants/api';


const BadRequest: FC = () => {
  const location = useLocation();
  const state = location.state as ErrorComponentLocationState | null;
  const message = state?.message || 'Something went wrong with your request. Please try again.';

  return (
    <div className="d-flex align-items-center justify-content-center vh-100">
      <div className="text-center">
        <img
          src={image}
          alt="Bad request illustration"
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

export default BadRequest;

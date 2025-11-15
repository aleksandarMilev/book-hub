import { HttpStatusCode } from 'axios';
import type { FC } from 'react';
import { Navigate } from 'react-router-dom';

import type { ErrorComponentLocationState } from '@/shared/components/errors/types/errorComponentLocationState.js';
import { routes } from '@/shared/lib/constants/api.js';
import type { HttpError } from '@/shared/types/errors/httpError.js';

export const ErrorRedirect: FC<{ error: HttpError }> = ({ error }) => {
  let redirectPath = routes.badRequest;

  switch (error.status) {
    case HttpStatusCode.Unauthorized:
    case HttpStatusCode.Forbidden:
      redirectPath = routes.accessDenied;
      break;

    case HttpStatusCode.NotFound:
      redirectPath = routes.notFound;
      break;

    default:
      redirectPath = routes.badRequest;
      break;
  }

  const locationState: ErrorComponentLocationState = { message: error.message };
  return <Navigate to={redirectPath} state={locationState} replace />;
};

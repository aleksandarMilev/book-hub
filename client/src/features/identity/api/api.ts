import type {
  LoginRequest,
  LoginResponse,
  RegisterRequest,
} from '@/features/identity/types/identity.js';
import { http, processError } from '@/shared/api/http.js';
import { routes } from '@/shared/lib/constants/api.js';
import { errors } from '@/shared/lib/constants/errorMessages.js';

export const register = async (request: RegisterRequest, signal?: AbortSignal) => {
  try {
    const url = `${routes.register}`;
    const formData = new FormData();

    formData.append('Username', request.username);
    formData.append('Email', request.email);
    formData.append('Password', request.password);
    formData.append('FirstName', request.firstName);
    formData.append('LastName', request.lastName);
    formData.append('DateOfBirth', request.dateOfBirth);

    if (request.socialMediaUrl && request.socialMediaUrl.trim() !== '') {
      formData.append('SocialMediaUrl', request.socialMediaUrl.trim());
    }

    if (request.biography && request.biography.trim() !== '') {
      formData.append('Biography', request.biography.trim());
    }

    formData.append('IsPrivate', String(request.isPrivate));

    if (request.image) {
      formData.append('Image', request.image);
    }

    const config = signal ? { signal } : undefined;
    const { data } = await http.post<LoginResponse>(url, formData, config);

    return data;
  } catch (error) {
    processError(error, errors.identity.register);
  }
};

export const login = async (
  credentials: string,
  password: string,
  rememberMe: boolean,
  signal?: AbortSignal,
) => {
  try {
    const url = `${routes.login}`;
    const payload: LoginRequest = { credentials, password, rememberMe };
    const { data } = await http.post<LoginResponse>(url, payload, getConfig(signal));

    return data;
  } catch (error) {
    processError(error, errors.identity.login);
  }
};

const getConfig = (signal?: AbortSignal) => {
  return {
    headers: { 'Content-Type': 'application/json' },
    ...(signal ? { signal } : {}),
  };
};

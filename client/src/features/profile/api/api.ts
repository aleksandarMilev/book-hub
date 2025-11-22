import type { CreateProfile, PrivateProfile, Profile } from '@/features/profile/types/profile.js';
import { getAuthConfig, http, httpAdmin, processError } from '@/shared/api/http.js';
import { routes } from '@/shared/lib/constants/api.js';
import { baseErrors, errors } from '@/shared/lib/constants/errorMessages.js';
import { isNotFoundError } from '@/shared/lib/utils/utils.js';

export const hasProfile = async (token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.hasProfile}`;
    const { data } = await http.get<boolean>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, baseErrors.general);
  }
};

export const topThree = async (signal?: AbortSignal) => {
  try {
    const url = `${routes.topProfiles}`;
    const config = signal ? { signal } : undefined;
    const { data } = await http.get<Profile[]>(url, config);

    return data;
  } catch (error) {
    processError(error, errors.profile.topThree);
  }
};

export const mine = async (token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.mineProfile}`;
    const { data } = await http.get<Profile>(url, getAuthConfig(token, signal));

    return data;
  } catch (error: unknown) {
    if (isNotFoundError(error)) {
      return null;
    }

    processError(error, errors.profile.byId);
  }
};

export const other = async (id: string, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.profile}/${id}`;
    const { data } = await http.get<Profile | PrivateProfile>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    processError(error, errors.profile.byId);
  }
};

export const create = async (profile: CreateProfile, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.profile}`;
    await http.post(url, profile, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.profile.create);
  }
};

export const edit = async (profile: CreateProfile, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.profile}`;
    await http.put(url, profile, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.profile.edit);
  }
};

export const remove = async (token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.profile}`;
    await http.delete(url, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.profile.delete);
  }
};

export const removeAsAdmin = async (id: string, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.profile}/${id}`;
    await httpAdmin.delete(url, getAuthConfig(token, signal));

    return true;
  } catch (error) {
    processError(error, errors.profile.delete);
  }
};

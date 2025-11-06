import { routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';
import { getAuthConfig, returnIfRequestCanceled } from '../common/utils';
import { http, httpAdmin } from '../common/http';
import type { Profile, ProfileInput, ProfileSummary } from './types/profile';
import { HttpStatusCode } from 'axios';

export async function names(token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.profile}`;
    const { data } = await http.get<ProfileSummary[]>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.profile.names);
    throw error;
  }
}

export async function hasProfile(token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.hasProfile}`;
    const { data } = await http.get<boolean>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.profile.get);
    throw error;
  }
}

export async function topThree(signal?: AbortSignal) {
  try {
    const url = `${routes.topProfiles}`;
    const config = signal ? { signal } : undefined;
    const { data } = await http.get<ProfileSummary[]>(url, config);

    return data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.profile.topThree);
    throw error;
  }
}

export async function mine(token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.mineProfile}`;
    const { data } = await http.get<Profile>(url, getAuthConfig(token, signal));

    return data;
  } catch (error: any) {
    if (error?.response?.status === HttpStatusCode.NotFound) {
      return null;
    }

    returnIfRequestCanceled(error, errors.profile.get);
    throw error;
  }
}

export async function other(id: string, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.profile}/${id}`;
    const { data } = await http.get<Profile>(url, getAuthConfig(token, signal));

    return data;
  } catch (error) {
    returnIfRequestCanceled(error, errors.profile.getOther);
    throw error;
  }
}

export async function create(profile: ProfileInput, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.profile}`;
    await http.post(url, profile, getAuthConfig(token, signal));
  } catch (error) {
    returnIfRequestCanceled(error, errors.profile.create);
    throw error;
  }
}

export async function edit(profile: ProfileInput, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.profile}`;
    await http.put(url, profile, getAuthConfig(token, signal));
  } catch (error) {
    returnIfRequestCanceled(error, errors.profile.edit);
    throw error;
  }
}

export async function remove(token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.profile}`;
    await http.delete(url, getAuthConfig(token, signal));
  } catch (error) {
    returnIfRequestCanceled(error, errors.profile.delete);
    throw error;
  }
}

export async function removeAsAdmin(id: string, token: string, signal?: AbortSignal) {
  try {
    const url = `${routes.profile}/${id}`;
    await httpAdmin.delete(url, getAuthConfig(token, signal));
  } catch (error) {
    returnIfRequestCanceled(error, errors.profile.delete);
    throw error;
  }
}

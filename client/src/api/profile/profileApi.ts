import axios, { HttpStatusCode } from 'axios';
import { baseUrl, baseAdminUrl, routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';
import { getAuthConfig } from '../common/utils';
import type { BaseProfile } from './types/baseProfile';
import type { Profile } from './types/profile';
import type { CreateProfile } from './types/createProfile';

export async function names(token: string) {
  try {
    const url = `${baseUrl}${routes.profile}`;
    const response = await axios.get<BaseProfile[]>(url, getAuthConfig(token));

    return response.data;
  } catch {
    throw new Error(errors.profile.names);
  }
}

export async function hasProfile(token: string) {
  try {
    const url = `${baseUrl}${routes.hasProfile}`;
    const response = await axios.get<boolean>(url, getAuthConfig(token));

    return response.data;
  } catch {
    throw new Error(errors.profile.get);
  }
}

export async function topThree() {
  try {
    const url = `${baseUrl}${routes.topProfiles}`;
    const response = await axios.get<Profile[]>(url);

    return response.data;
  } catch {
    throw new Error(errors.profile.topThree);
  }
}

export async function mine(token: string) {
  try {
    const url = `${baseUrl}${routes.mineProfile}`;
    const response = await axios.get<Profile>(url, getAuthConfig(token));

    return response.data;
  } catch (error: any) {
    if (error?.response?.status === HttpStatusCode.NotFound) {
      return null;
    }

    throw new Error(errors.profile.get);
  }
}

export async function other(id: string, token: string) {
  try {
    const url = `${baseUrl}${routes.profile}/${id}`;
    const response = await axios.get<BaseProfile | Profile>(url, getAuthConfig(token));

    return response.data;
  } catch {
    throw new Error(errors.profile.getOther);
  }
}

export async function create(profile: CreateProfile, token: string) {
  try {
    const url = `${baseUrl}${routes.profile}`;
    await axios.post(url, profile, getAuthConfig(token));
  } catch {
    throw new Error(errors.profile.create);
  }
}

export async function edit(profile: CreateProfile, token: string) {
  try {
    const url = `${baseUrl}${routes.profile}`;
    await axios.put(url, profile, getAuthConfig(token));
  } catch {
    throw new Error(errors.profile.edit);
  }
}

export async function remove(token: string) {
  try {
    const url = `${baseUrl}${routes.profile}`;
    await axios.delete(url, getAuthConfig(token));
  } catch {
    throw new Error(errors.profile.delete);
  }
}

export async function removeAsAdmin(id: string, token: string) {
  try {
    const url = `${baseAdminUrl}${routes.profile}/${id}`;
    await axios.delete(url, getAuthConfig(token));
  } catch {
    throw new Error(errors.profile.delete);
  }
}

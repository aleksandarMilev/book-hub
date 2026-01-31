import type { CreateProfile, PrivateProfile, Profile } from '@/features/profile/types/profile';
import { getAuthConfig, http, httpAdmin, processError } from '@/shared/api/http';
import { routes } from '@/shared/lib/constants/api';
import { errors } from '@/shared/lib/constants/errorMessages';
import { isNotFoundError } from '@/shared/lib/utils/utils';

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
  } catch (error) {
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

export const edit = async (profile: CreateProfile, token: string, signal?: AbortSignal) => {
  try {
    const url = `${routes.profile}`;
    const formData = new FormData();

    formData.append('FirstName', profile.firstName);
    formData.append('LastName', profile.lastName);

    if (profile.dateOfBirth) {
      formData.append('DateOfBirth', profile.dateOfBirth);
    }

    if (profile.socialMediaUrl) {
      formData.append('SocialMediaUrl', profile.socialMediaUrl);
    }

    if (profile.biography) {
      formData.append('Biography', profile.biography);
    }

    formData.append('IsPrivate', String(profile.isPrivate));

    if (profile.image) {
      formData.append('Image', profile.image);
    }

    if (profile.removeImage) {
      formData.append('RemoveImage', String(profile.removeImage));
    }

    const config = {
      headers: {
        Authorization: `Bearer ${token}`,
      },
      ...(signal ? { signal } : {}),
    };

    await http.put(url, formData, config);

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



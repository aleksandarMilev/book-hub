import { useCallback, useContext, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { format } from 'date-fns';

import * as api from '../api/profile/profileApi';
import { routes } from '../common/constants/api';
import { UserContext } from '../contexts/user/userContext';
import type { Profile } from '../api/profile/types/profile';
import type { BaseProfile } from '../api/profile/types/baseProfile';
import type { CreateProfile } from '../api/profile/types/createProfile';

export function useTopThree() {
  const [profiles, setProfiles] = useState<Profile[] | null>(null);
  const [isFetching, setIsFetching] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchData = useCallback(async () => {
    try {
      setIsFetching(true);
      const data = await api.topThree();
      setProfiles(data);
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Failed to load profiles.';
      setError(message);
    } finally {
      setIsFetching(false);
    }
  }, []);

  useEffect(() => {
    void fetchData();
  }, [fetchData]);

  return { profiles, isFetching, error, refetch: fetchData };
}

export function useMineProfile() {
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const [profile, setProfile] = useState<Profile | null>(null);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(async () => {
    if (!token) {
      return;
    }

    try {
      setIsFetching(true);
      const profileData = await api.mine(token);
      if (profileData) {
        const formattedProfile: Profile = {
          ...profileData,
          dateOfBirth: format(new Date(profileData.dateOfBirth), 'yyyy-MM-dd'),
        };

        setProfile(formattedProfile);
      } else {
        setProfile(null);
      }
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Failed to load your profile.';
      navigate(routes.badRequest, { state: { message } });
    } finally {
      setIsFetching(false);
    }
  }, [token, navigate]);

  useEffect(() => {
    void fetchData();
  }, [fetchData]);

  return { profile, isFetching, refetch: fetchData };
}

export function useOtherProfile(id: string) {
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const [profile, setProfile] = useState<BaseProfile | Profile | null>(null);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(async () => {
    if (!id || !token) {
      return;
    }

    try {
      setIsFetching(true);

      const data = await api.other(id, token);
      const formattedProfile =
        'dateOfBirth' in data && data.dateOfBirth
          ? {
              ...data,
              dateOfBirth: format(new Date(data.dateOfBirth), 'yyyy-MM-dd'),
            }
          : data;

      setProfile(formattedProfile);
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Failed to load user profile.';
      navigate(routes.badRequest, { state: { message } });
    } finally {
      setIsFetching(false);
    }
  }, [id, token, navigate]);

  useEffect(() => {
    void fetchData();
  }, [fetchData]);

  return { profile, isFetching, refetch: fetchData };
}

export function useCreate() {
  const { token, changeHasProfileState } = useContext(UserContext);
  const navigate = useNavigate();

  const createHandler = useCallback(
    async (profileData: CreateProfile) => {
      if (!token) {
        return;
      }

      const profile: CreateProfile = {
        ...profileData,
        imageUrl: profileData.imageUrl || null,
        socialMediaUrl: profileData.socialMediaUrl || null,
        biography: profileData.biography || null,
      };

      try {
        await api.create(profile, token);
        changeHasProfileState(true);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to create profile.';
        navigate(routes.badRequest, { state: { message } });
      }
    },
    [token, navigate, changeHasProfileState],
  );

  return createHandler;
}

export function useEdit() {
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  const editHandler = useCallback(
    async (profileData: CreateProfile) => {
      if (!token) {
        return;
      }

      const profile: CreateProfile = {
        ...profileData,
        imageUrl: profileData.imageUrl || null,
        socialMediaUrl: profileData.socialMediaUrl || null,
        biography: profileData.biography || null,
      };

      try {
        await api.edit(profile, token);
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to edit profile.';
        navigate(routes.badRequest, { state: { message } });
      }
    },
    [token, navigate],
  );

  return editHandler;
}

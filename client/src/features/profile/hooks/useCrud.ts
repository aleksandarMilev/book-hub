import { format } from 'date-fns';
import { useCallback, useEffect, useMemo, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';

import * as api from '@/features/profile/api/api';
import type { CreateProfile, PrivateProfile, Profile } from '@/features/profile/types/profile';
import { useList } from '@/features/reading-list/hooks/useCrud';
import { useChatsNotJoined } from '@/hooks/useChat';
import { routes } from '@/shared/lib/constants/api';
import { IsCanceledError, IsError } from '@/shared/lib/utils';
import { useAuth } from '@/shared/stores/auth/auth';

const isFullProfile = (profile: Profile | PrivateProfile | null): profile is Profile =>
  !!profile && 'phoneNumber' in profile;

const toCreateType = (profile: Profile): CreateProfile => ({
  firstName: profile.firstName,
  lastName: profile.lastName,
  imageUrl: profile.imageUrl ?? '',
  phoneNumber: profile.phoneNumber ?? '',
  dateOfBirth: profile.dateOfBirth ? format(new Date(profile.dateOfBirth), 'yyyy-MM-dd') : '',
  socialMediaUrl: profile.socialMediaUrl ?? null,
  biography: profile.biography ?? null,
  isPrivate: !!profile.isPrivate,
});

const useProfile = (otherId?: string) => {
  const { token } = useAuth();
  const navigate = useNavigate();

  const [profile, setProfile] = useState<Profile | PrivateProfile | null>(null);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(
    async (signal?: AbortSignal) => {
      if (!token) {
        return;
      }

      try {
        setIsFetching(true);

        if (otherId) {
          const data = await api.other(otherId, token, signal);
          if (data) {
            if (isFullProfile(data)) {
              setProfile({
                ...data,
                dateOfBirth: data.dateOfBirth
                  ? format(new Date(data.dateOfBirth), 'yyyy-MM-dd')
                  : '',
              });
            } else {
              setProfile(data);
            }
          } else {
            setProfile(null);
          }
        } else {
          const mine = await api.mine(token, signal);
          if (mine) {
            setProfile({
              ...mine,
              dateOfBirth: mine.dateOfBirth ? format(new Date(mine.dateOfBirth), 'yyyy-MM-dd') : '',
            });
          } else {
            setProfile(null);
          }
        }
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const message = IsError(error) ? error.message : 'Failed to load profile.';
        navigate(routes.badRequest, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    },
    [token, otherId, navigate],
  );

  useEffect(() => {
    const controller = new AbortController();
    void fetchData(controller.signal);
    return () => controller.abort();
  }, [fetchData]);

  const profileInput = useMemo(
    () => (isFullProfile(profile) ? toCreateType(profile) : null),
    [profile],
  );

  return { profile, profileInput, isFetching, refetch: fetchData };
};

export const useDetails = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { token, userId, isAdmin } = useAuth();

  const otherId = (location.state as { id?: string } | null)?.id;
  const { profile, isFetching: profileLoading } = useProfile(otherId);
  const { showModal, toggleModal, deleteHandler } = useRemove(profile?.id);
  const {
    chatNames: chatButtons,
    isFetching: chatLoading,
    error: chatError,
    refetch: refetchChats,
  } = useChatsNotJoined(otherId ?? undefined);

  const { readingList, isFetching: readingLoading } = useList(
    'currently reading',
    null,
    null,
    profile?.isPrivate,
    profile?.id,
  );

  const canSeePrivate = !!profile && (!profile.isPrivate || profile.id === userId);

  const getNavigateState = useCallback(
    (status: string) => ({
      state: {
        id: otherId ?? profile?.id,
        readingListStatus: status,
        firstName: profile?.firstName,
      },
    }),
    [otherId, profile?.id, profile?.firstName],
  );

  const onNavigateToRead = useCallback(
    () => navigate(routes.readingList, getNavigateState('toRead')),
    [navigate, getNavigateState],
  );

  const onNavigateRead = useCallback(
    () => navigate(routes.readingList, getNavigateState('read')),
    [navigate, getNavigateState],
  );

  return useMemo(
    () => ({
      token,
      userId,
      isAdmin,
      profile,
      canSeePrivate,
      profileLoading,
      readingList,
      readingLoading,
      showModal,
      toggleModal,
      deleteHandler,
      chatButtons,
      chatLoading,
      chatError,
      refetchChats,
      onNavigateRead,
      onNavigateToRead,
    }),
    [
      token,
      userId,
      isAdmin,
      profile,
      canSeePrivate,
      profileLoading,
      readingList,
      readingLoading,
      showModal,
      toggleModal,
      deleteHandler,
      chatButtons,
      chatLoading,
      chatError,
      refetchChats,
      onNavigateRead,
      onNavigateToRead,
    ],
  );
};

export const useTopProfiles = () => {
  const [profiles, setProfiles] = useState<Profile[]>([]);
  const [isFetching, setIsFetching] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchData = useCallback(async (signal?: AbortSignal) => {
    try {
      setIsFetching(true);
      setError(null);

      const data = await api.topThree(signal);
      setProfiles(data ?? []);
    } catch (error) {
      if (IsCanceledError(error)) {
        return;
      }

      const message = IsError(error) ? error.message : 'Failed to load profiles.';
      setError(message);
    } finally {
      setIsFetching(false);
    }
  }, []);

  useEffect(() => {
    const controller = new AbortController();
    void fetchData(controller.signal);
    return () => controller.abort();
  }, [fetchData]);

  return { profiles, isFetching, error, refetch: fetchData };
};

export const useOtherProfile = (id?: string | null) => useProfile(id ?? undefined);

export const useMineProfile = () => {
  const { token } = useAuth();
  const navigate = useNavigate();

  const [profile, setProfile] = useState<Profile | null>(null);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(
    async (signal?: AbortSignal) => {
      if (!token) {
        return;
      }

      try {
        setIsFetching(true);
        const data = await api.mine(token, signal);
        if (data) {
          setProfile({
            ...data,
            dateOfBirth: data.dateOfBirth ? format(new Date(data.dateOfBirth), 'yyyy-MM-dd') : '',
          });
        } else {
          setProfile(null);
        }
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const message = IsError(error) ? error.message : 'Failed to load your profile.';
        navigate(routes.badRequest, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    },
    [token, navigate],
  );

  useEffect(() => {
    const controller = new AbortController();
    void fetchData(controller.signal);
    return () => controller.abort();
  }, [fetchData]);

  const profileInput = useMemo(() => (profile ? toCreateType(profile) : null), [profile]);
  return { profile, profileInput, isFetching, refetch: fetchData };
};

export const useCreate = () => {
  const navigate = useNavigate();
  const { token, changeHasProfileState } = useAuth();

  return useCallback(
    async (values: CreateProfile): Promise<boolean | undefined> => {
      if (!token) {
        return undefined;
      }

      const toSend: CreateProfile = {
        ...values,
        socialMediaUrl: values.socialMediaUrl ?? null,
        biography: values.biography ?? null,
      };

      try {
        await api.create(toSend, token);
        changeHasProfileState?.(true);

        return true;
      } catch (error) {
        const message = IsError(error) ? error.message : 'Failed to create profile.';
        navigate(routes.badRequest, { state: { message } });

        return undefined;
      }
    },
    [token, navigate, changeHasProfileState],
  );
};

export const useEdit = () => {
  const { token } = useAuth();
  const navigate = useNavigate();

  return useCallback(
    async (values: CreateProfile): Promise<boolean | undefined> => {
      if (!token) {
        return undefined;
      }

      const toSend: CreateProfile = {
        ...values,
        socialMediaUrl: values.socialMediaUrl ?? null,
        biography: values.biography ?? null,
      };

      try {
        return await api.edit(toSend, token);
      } catch (error) {
        const message = IsError(error) ? error.message : 'Failed to edit profile.';
        navigate(routes.badRequest, { state: { message } });

        return undefined;
      }
    },
    [token, navigate],
  );
};

export const useRemove = (id?: string) => {
  const navigate = useNavigate();
  const { token, isAdmin } = useAuth();

  const [showModal, setShowModal] = useState(false);
  const toggleModal = useCallback(() => setShowModal((prev) => !prev), []);

  const deleteHandler = useCallback(async () => {
    if (!token) {
      return;
    }

    if (!showModal) {
      toggleModal();
      return;
    }

    const controller = new AbortController();

    try {
      if (id && isAdmin) {
        await api.removeAsAdmin(id, token, controller.signal);
      } else {
        await api.remove(token, controller.signal);
      }

      navigate(routes.home);
    } catch (error) {
      if (IsCanceledError(error)) {
        return;
      }

      const message = IsError(error) ? error.message : 'Failed to delete profile.';
      throw new Error(message);
    } finally {
      toggleModal();
      controller.abort();
    }
  }, [token, id, isAdmin, navigate, showModal, toggleModal]);

  return { showModal, toggleModal, deleteHandler };
};

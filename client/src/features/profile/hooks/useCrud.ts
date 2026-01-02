import { useCallback, useEffect, useMemo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLocation, useNavigate } from 'react-router-dom';

import { useChatsNotJoined } from '@/features/chat/hooks/useCrud.js';
import * as api from '@/features/profile/api/api.js';
import type { CreateProfile, PrivateProfile, Profile } from '@/features/profile/types/profile.js';
import { useList } from '@/features/reading-list/hooks/useCrud.js';
import { routes } from '@/shared/lib/constants/api.js';
import { IsCanceledError, IsError } from '@/shared/lib/utils/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

const useProfile = (otherId?: string) => {
  const { token } = useAuth();
  const navigate = useNavigate();
  const { t } = useTranslation('profiles');

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
          setProfile(await api.other(otherId, token, signal));
        } else {
          setProfile(await api.mine(token, signal));
        }
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const message = IsError(error) ? error.message : t('messages.profileLoadFailed');
        navigate(routes.badRequest, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    },
    [token, otherId, navigate, t],
  );

  useEffect(() => {
    const controller = new AbortController();
    void fetchData(controller.signal);
    return () => controller.abort();
  }, [fetchData]);

  return { profile, isFetching, refetch: fetchData };
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
  const { t } = useTranslation('profiles');

  const fetchData = useCallback(
    async (signal?: AbortSignal) => {
      try {
        setIsFetching(true);
        setError(null);

        const data = await api.topThree(signal);
        setProfiles(data ?? []);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const message = IsError(error) ? error.message : t('messages.profilesLoadFailed');
        setError(message);
      } finally {
        setIsFetching(false);
      }
    },
    [t],
  );

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
  const { t } = useTranslation('profiles');

  const [profile, setProfile] = useState<Profile | null>(null);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(
    async (signal?: AbortSignal) => {
      if (!token) {
        return;
      }

      try {
        setIsFetching(true);
        setProfile(await api.mine(token, signal));
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const message = IsError(error) ? error.message : t('messages.ownProfileLoadFailed');
        navigate(routes.badRequest, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    },
    [token, navigate, t],
  );

  useEffect(() => {
    const controller = new AbortController();
    void fetchData(controller.signal);
    return () => controller.abort();
  }, [fetchData]);

  return { profile, isFetching, refetch: fetchData };
};

export const useEdit = () => {
  const { token } = useAuth();
  const navigate = useNavigate();
  const { t } = useTranslation('profiles');

  return useCallback(
    async (values: CreateProfile): Promise<boolean | undefined> => {
      if (!token) {
        return undefined;
      }

      const toSend: CreateProfile = {
        firstName: values.firstName.trim(),
        lastName: values.lastName.trim(),
        dateOfBirth:
          values.dateOfBirth && values.dateOfBirth.trim() !== '' ? values.dateOfBirth.trim() : null,
        socialMediaUrl: values.socialMediaUrl?.trim() || null,
        biography: values.biography?.trim() || null,
        isPrivate: values.isPrivate,
        image: values.image ?? null,
        removeImage: values.removeImage ?? false,
      };

      try {
        return await api.edit(toSend, token);
      } catch (error) {
        const message = IsError(error) ? error.message : t('messages.editFailedSimple');
        navigate(routes.badRequest, { state: { message } });

        return undefined;
      }
    },
    [token, navigate, t],
  );
};

export const useRemove = (id?: string) => {
  const navigate = useNavigate();
  const { token, isAdmin } = useAuth();
  const { t } = useTranslation('profiles');

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

      const message = IsError(error) ? error.message : t('messages.deleteFailed');
      throw new Error(message);
    } finally {
      toggleModal();
      controller.abort();
    }
  }, [token, id, isAdmin, navigate, showModal, toggleModal, t]);

  return { showModal, toggleModal, deleteHandler };
};

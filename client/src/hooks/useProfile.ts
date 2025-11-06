import { useCallback, useContext, useEffect, useMemo, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { format } from 'date-fns';
import * as api from '../api/profile/profileApi';
import * as chatHooks from './useChat';
import * as readingListHooks from './useReadingList';
import { routes } from '../common/constants/api';
import { UserContext } from '../contexts/user/userContext';
import type { Profile, ProfileInput, ProfileSummary } from '../api/profile/types/profile';

function toInput(profile: Profile): ProfileInput {
  return {
    firstName: profile.firstName,
    lastName: profile.lastName,
    imageUrl: profile.imageUrl ?? null,
    phoneNumber: profile.phoneNumber ?? '',
    dateOfBirth: profile.dateOfBirth ? format(new Date(profile.dateOfBirth), 'yyyy-MM-dd') : '',
    socialMediaUrl: profile.socialMediaUrl ?? null,
    biography: profile.biography ?? null,
    isPrivate: !!profile.isPrivate,
  };
}

export function useDetails() {
  const location = useLocation();
  const navigate = useNavigate();
  const { token, userId, isAdmin } = useContext(UserContext);

  const otherId = (location.state as { id?: string } | null)?.id;
  const { profile, isFetching: profileLoading } = otherId
    ? useOtherProfile(otherId)
    : useMineProfile();

  const { showModal, toggleModal, deleteHandler } = useRemove(profile?.id);
  const {
    chatNames: chatButtons,
    isFetching: chatLoading,
    error: chatError,
    refetch: refetchChats,
  } = chatHooks.useChatsNotJoined(otherId ?? undefined);

  const { readingList, isFetching: readingLoading } = readingListHooks.useList(
    'currentlyReading',
    null,
    null,
    profile?.isPrivate,
    profile?.id,
  );

  const canSeePrivate = !!profile && (!profile.isPrivate || profile.id === userId);

  const getNavigateState = (status: string) => ({
    state: { id: otherId ?? profile?.id, readingListStatus: status, firstName: profile?.firstName },
  });

  const onNavigateToRead = () => navigate(routes.readingList, getNavigateState('toRead'));
  const onNavigateRead = () => navigate(routes.readingList, getNavigateState('read'));

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
}

export function useTopProfiles() {
  const [profiles, setProfiles] = useState<ProfileSummary[]>([]);
  const [isFetching, setIsFetching] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchData = useCallback(async (signal?: AbortSignal) => {
    try {
      setIsFetching(true);
      setError(null);

      const data = await api.topThree(signal);

      setProfiles(data);
    } catch (error) {
      if (error instanceof DOMException && error.name === 'AbortError') {
        return;
      }

      const message = error instanceof Error ? error.message : 'Failed to load profiles.';
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
}

export function useMineProfile() {
  const navigate = useNavigate();
  const { token } = useContext(UserContext);

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
        if (error instanceof DOMException && error.name === 'AbortError') {
          return;
        }

        const message = error instanceof Error ? error.message : 'Failed to load your profile.';
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

  const profileInput = useMemo(() => (profile ? toInput(profile) : null), [profile]);
  return { profile, profileInput, isFetching, refetch: fetchData };
}

export function useOtherProfile(id?: string | null) {
  const navigate = useNavigate();
  const { token } = useContext(UserContext);

  const [profile, setProfile] = useState<Profile | null>(null);
  const [isFetching, setIsFetching] = useState(false);

  const fetchData = useCallback(
    async (signal?: AbortSignal) => {
      if (!id || !token) {
        return;
      }

      try {
        setIsFetching(true);

        const data = await api.other(id, token, signal);
        setProfile({
          ...data,
          dateOfBirth: data.dateOfBirth ? format(new Date(data.dateOfBirth), 'yyyy-MM-dd') : '',
        });
      } catch (error) {
        if (error instanceof DOMException && error.name === 'AbortError') {
          return;
        }

        const message = error instanceof Error ? error.message : 'Failed to load user profile.';
        navigate(routes.badRequest, { state: { message } });
      } finally {
        setIsFetching(false);
      }
    },
    [id, token, navigate],
  );

  useEffect(() => {
    const controller = new AbortController();
    void fetchData(controller.signal);

    return () => controller.abort();
  }, [fetchData]);

  const profileInput = useMemo(() => (profile ? toInput(profile) : null), [profile]);
  return { profile, profileInput, isFetching, refetch: fetchData };
}

export function useCreate() {
  const navigate = useNavigate();
  const { token, changeHasProfileState } = useContext(UserContext);

  return useCallback(
    async (values: ProfileInput) => {
      if (!token) {
        return;
      }

      const toSend: ProfileInput = {
        ...values,
        imageUrl: values.imageUrl || null,
        socialMediaUrl: values.socialMediaUrl || null,
        biography: values.biography || null,
      };

      try {
        await api.create(toSend, token);
        changeHasProfileState?.(true);
        7;

        return true;
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to create profile.';
        navigate(routes.badRequest, { state: { message } });

        throw error;
      }
    },
    [token, navigate, changeHasProfileState],
  );
}

export function useEdit() {
  const navigate = useNavigate();
  const { token } = useContext(UserContext);

  return useCallback(
    async (values: ProfileInput) => {
      if (!token) {
        return;
      }

      const toSend: ProfileInput = {
        ...values,
        imageUrl: values.imageUrl || null,
        socialMediaUrl: values.socialMediaUrl || null,
        biography: values.biography || null,
      };

      try {
        await api.edit(toSend, token);
        return true;
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Failed to edit profile.';
        navigate(routes.badRequest, { state: { message } });

        throw error;
      }
    },
    [token, navigate],
  );
}

export function useRemove(id?: string) {
  const navigate = useNavigate();
  const { token, isAdmin } = useContext(UserContext);

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
      if (error instanceof DOMException && error.name === 'AbortError') {
        return;
      }

      const message = error instanceof Error ? error.message : 'Failed to delete profile.';
      throw new Error(message);
    } finally {
      toggleModal();
    }
  }, [token, id, isAdmin, navigate, showModal, toggleModal]);

  return { showModal, toggleModal, deleteHandler };
}

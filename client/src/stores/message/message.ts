import { create } from 'zustand';

import type { MessageState } from './types/messageState.type';
import type { TimeoutHandle } from './types/timeoutHandle.type';

let hideTimer: TimeoutHandle = null;

export const useMessageStore = create<MessageState>((set) => ({
  message: null,
  isSuccess: true,

  show: (message, isSuccess = true, durationMs = 5_000) => {
    if (hideTimer) {
      clearTimeout(hideTimer);
      hideTimer = null;
    }

    set({ message, isSuccess });

    hideTimer = setTimeout(() => {
      set({ message: null });
      hideTimer = null;
    }, durationMs);
  },

  clear: () => {
    if (hideTimer) {
      clearTimeout(hideTimer);
      hideTimer = null;
    }
    set({ message: null });
  },
}));

export const selectMessage = (state: MessageState) => state.message;

export const selectIsSuccess = (state: MessageState) => state.isSuccess;

export const selectShow = (state: MessageState) => state.show;

export const selectClear = (state: MessageState) => state.clear;

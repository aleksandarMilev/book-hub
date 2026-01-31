import { shallow } from 'zustand/shallow';
import { createWithEqualityFn } from 'zustand/traditional';

import type { MessageState } from '@/shared/stores/message/types/messageState';
import type { MessageView } from '@/shared/stores/message/types/messageView';
import type { TimeoutHandle } from '@/shared/stores/message/types/timeoutHandle';

let hideTimer: TimeoutHandle = null;

const useMessageStore = createWithEqualityFn<MessageState>()(
  (set) => ({
    message: null,
    isSuccess: true,

    show: (message, isSuccess = true, durationMs = 7_500) => {
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
  }),
  shallow,
);

export const useMessage = (): MessageView =>
  useMessageStore(
    (state) => ({
      showMessage: state.show,
      clearMessage: state.clear,
      isShowing: Boolean(state.message),
      isSuccess: state.isSuccess,
      message: state.message,
    }),
    shallow,
  );



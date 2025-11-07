import { createContext, useMemo, useCallback, useContext, type PropsWithChildren } from 'react';

import type { MessageContextValue } from './types/messageContextValue.type';

import MessageDisplay from '../../components/common/message/Message';
import {
  selectIsSuccess,
  selectMessage,
  selectShow,
  useMessageStore,
} from '../../stores/message/message';


export const MessageContext = createContext<MessageContextValue>({
  showMessage: () => {},
});

export function MessageProvider({ children }: PropsWithChildren) {
  const message = useMessageStore(selectMessage);
  const isSuccess = useMessageStore(selectIsSuccess);
  const show = useMessageStore(selectShow);

  const showMessage = useCallback<MessageContextValue['showMessage']>(
    (message, ok = true, durationMs) => show(message, ok, durationMs),
    [show],
  );

  const value = useMemo<MessageContextValue>(() => ({ showMessage }), [showMessage]);

  return (
    <MessageContext.Provider value={value}>
      {children}
      {message && <MessageDisplay message={message} isSuccess={isSuccess} />}
    </MessageContext.Provider>
  );
}

export const useMessage = () => useContext(MessageContext);

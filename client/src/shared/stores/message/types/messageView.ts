export type MessageView = {
  showMessage: (message: string, isSuccess?: boolean, durationMs?: number) => void;
  clearMessage: () => void;
  isShowing: boolean;
  isSuccess: boolean;
};

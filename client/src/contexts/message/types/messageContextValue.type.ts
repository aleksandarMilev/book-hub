export interface MessageContextValue {
  showMessage: (message: string, isSuccess?: boolean, durationMs?: number) => void;
}

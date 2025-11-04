export interface MessageState {
  message: string | null;
  isSuccess: boolean;
  show: (message: string, isSuccess?: boolean, durationMs?: number) => void;
  clear: () => void;
}

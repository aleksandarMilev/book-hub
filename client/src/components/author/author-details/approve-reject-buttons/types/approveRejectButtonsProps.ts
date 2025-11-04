export interface ApproveRejectButtonsProps {
  authorId: number;
  authorName: string;
  initialIsApproved: boolean;
  token: string;
  onSuccess: (message: string, success?: boolean) => void;
}

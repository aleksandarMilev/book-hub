export interface PaginationProps {
  page: number;
  totalPages: number;
  disabled?: boolean;
  onPageChange: (newPage: number) => void;
}

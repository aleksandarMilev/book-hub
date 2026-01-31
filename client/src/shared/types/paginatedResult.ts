export type PaginatedResult<T> = {
  items: T[];
  totalItems: number;
  pageIndex: number;
  pageSize: number;
};


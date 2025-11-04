export interface JwtPayload {
  nameid: string;
  unique_name: string;
  email: string;
  role?: string;
}

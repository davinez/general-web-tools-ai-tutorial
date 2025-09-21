export interface ApiError {
  domain?: string;
  reason?: string;
  message?: string;
}

export interface ApiTopLevelError {
  code: number;
  message?: string;
  errors: ApiError[];
}

export interface ApiErrorResponse {
  apiVersion: string;
  error: ApiTopLevelError;
}

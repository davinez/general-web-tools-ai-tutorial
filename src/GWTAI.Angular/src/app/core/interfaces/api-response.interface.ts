export interface ApiResponse<T extends object | undefined = undefined> {
  data: T;
}

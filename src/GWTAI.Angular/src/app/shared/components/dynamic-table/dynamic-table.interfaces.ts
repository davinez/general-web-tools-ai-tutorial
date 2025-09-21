
export interface ColumnDef<T> {
  key: keyof T;
  label: string;
  editable?: boolean;
}

export interface PaginatorConfig {
  length: number;
  pageSize: number;
  pageSizeOptions: number[];
  idKey?: string; // Added for unique row identification
}

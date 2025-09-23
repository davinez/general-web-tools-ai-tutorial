export interface ColumnDef<T> {
  key: keyof T;
  label: string;
  editable?: boolean;
  pipe?: (value: any) => string; // Optional pipe function for formatting 
}

export interface PaginatorConfig {
  length: number;
  pageSize: number;
  pageSizeOptions: number[];
  idKey?: string; // Added for unique row identification
}

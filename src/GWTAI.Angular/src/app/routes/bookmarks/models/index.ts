import { FormControl } from "@angular/forms";
import { JobStatus, WorkflowType } from "@shared";

export interface UploadResponse {
  uploadId: string;
  isQueuePublishSuccess: boolean;
  message: string;
}

// Interface for the backend payload, matching the C# class
export interface BookmarksBulkUpload {
  FileName: string;
  FileContent: File;
  UploadTimestamp: string;
}

// Table Data Rows
export interface UploadEvent {
  uploadId: string;
  userId: string;
  /**
   * The UTC timestamp when the event was recorded.
   * @format date-time (ISO 8601 string)
   */
  eventTimestamp: string;
  status: JobStatus;
  /**
   * A serialized JSON string containing workflow-specific data.
   */
  content: string;
  /**
   * The workflow that triggered this event.
   */
  workflow: WorkflowType;
}


// Interface for the form model
export interface BookmarksUploadForm {
  fileName: FormControl<string | null>;
  fileContent: FormControl<File | null>;
}

// Interface for the SignalR payload
export interface JobEventStatusUpdate {
  jobId: string;
  status: string;
  timestamp: string;
}
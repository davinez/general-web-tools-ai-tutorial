import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UploadResponse, UploadEvent } from '../models';

@Injectable({
  providedIn: 'root',
})
export class BookmarkService {
  private http = inject(HttpClient);
  // Old way of using injection
  // constructor(private http: HttpClient) {}

  uploadBookmarks(formData: FormData): Observable<UploadResponse> {
    return this.http.post<UploadResponse>('/api/bookmarks/bulk-upload', formData);
  }

  getUploads(): Observable<UploadEvent[]> {
    return this.http.get<UploadEvent[]>('/api/jobevents?workflow=BookmarksUpload');
  }

  getUpload(jobId: string): Observable<UploadEvent[]> {
    return this.http.get<UploadEvent[]>(`/api/jobevents?workflow=BookmarksUpload&jobId=${jobId}`);
  }
}

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { UploadResponse, UploadEvent } from '../models';
import { ApiResponse } from '@core/interfaces/api-response.interface';

@Injectable({
  providedIn: 'root',
})
export class BookmarkService {
  private http = inject(HttpClient);
  // Old way of using injection
  // constructor(private http: HttpClient) {}

  uploadBookmarks(formData: FormData): Observable<UploadResponse> {
    return this.http.post<ApiResponse<UploadResponse>>('/bookmarks/bulk-upload', formData).pipe(
      map(response => response.data)
    );
  }

  getUploads(): Observable<UploadEvent[]> {
    return this.http.get<ApiResponse<UploadEvent[]>>(`/job-events?workflow=BookmarksUpload`).pipe(
      map(response => response.data)
    );
  }

  getUpload(id: string): Observable<UploadEvent> {
    return this.http.get<ApiResponse<UploadEvent>>(`/job-events/${id}?workflow=BookmarksUpload`).pipe(
      map(response => response.data)
    );
  }
}

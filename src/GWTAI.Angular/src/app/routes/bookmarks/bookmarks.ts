import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';

import { BookmarkService } from './services/bookmarks.service';
import { BookmarksBulkUpload, BookmarksUploadForm, JobEventStatusUpdate, UploadEvent, UploadResponse } from './models';
import { ApiErrorResponse } from '@core/interfaces/api-error.interface';
import { SignalRService } from '@shared/services/signalr.service';
import { DynamicTableComponent } from '@shared/components/dynamic-table/dynamic-table.component';
import { ColumnDef, PaginatorConfig } from '@shared/components/dynamic-table/dynamic-table.interfaces';
import { MatProgressBarModule } from '@angular/material/progress-bar';


@Component({
  selector: 'app-bookmarks',
  templateUrl: './bookmarks.html',
  styleUrls: ['./bookmarks.scss'],
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    DynamicTableComponent,
    MatProgressBarModule,
  ],
})
export class Bookmarks implements OnInit, OnDestroy {
  @ViewChild('fileInput') fileInput!: ElementRef;
  uploadForm: FormGroup<BookmarksUploadForm>;
  isUploading = false;
  public uploads: UploadEvent[] = [];
  public columns: ColumnDef<UploadEvent>[] = [
    { key: 'jobEventId', label: 'Upload ID' },
    { key: 'eventTimestamp', label: 'Timestamp', pipe: (value: string) => new Date(value).toLocaleDateString() },
    { key: 'status', label: 'Status', editable: true },
  ];
  public paginatorConfig: PaginatorConfig = {
    length: 0, pageSize: 10, pageSizeOptions: [5, 10, 25, 100], idKey: 'jobEventId'
  };
  private uploadsUpdateSubscription: Subscription;

  constructor(
    private fb: FormBuilder,
    private toastr: ToastrService,
    private bookmarkService: BookmarkService,
    private signalRService: SignalRService,
    private cdr: ChangeDetectorRef
  ) {
    this.uploadForm = this.fb.group<BookmarksUploadForm>({
      fileName: new FormControl(null, Validators.required),
      fileContent: new FormControl(null, Validators.required)
    });
    this.uploadsUpdateSubscription = new Subscription();
  }

  ngOnInit(): void {
    this.loadInitialJobs();
    this.setupSignalRListeners();
  }

  private loadInitialJobs(): void {
    this.bookmarkService.getUploads().subscribe(initialUploads => {
      this.uploads = initialUploads;
      this.paginatorConfig = { ...this.paginatorConfig, length: initialUploads.length };
      this.cdr.markForCheck();
    });
  }

  private async setupSignalRListeners(): Promise<void> {
    await this.signalRService.startConnection();
    // The backend sends a simple status update. We use the jobId from it
    // to fetch the complete, updated job event data.
    this.uploadsUpdateSubscription = this.signalRService
      // 'StatusUpdate' name of the event/method from the backend: await _hubContext.Clients.User(message.UserId).StatusUpdate(hubUpdate);
      .getEventObservable<JobEventStatusUpdate>('StatusUpdate')
      .subscribe(update => {
        if (!update?.jobEventId) {
          console.error('Invalid SignalR update received, jobEventId is missing:', update);
          return;
        }
        this.bookmarkService.getUpload(update.jobEventId).subscribe(updatedEvent => {
          // The length check was preventing new items from being added to an empty list.
          // We should process the event as long as it's valid.
          if (updatedEvent) {
            this.handleUploadUpdate(updatedEvent);
          } else {
            console.warn('getUpload returned null or undefined. Skipping UI update.');
          }
        });
      });
  }

  private handleUploadUpdate(updatedUpload: UploadEvent): void {
    const index = this.uploads.findIndex(u => u.jobEventId === updatedUpload.jobEventId);

    if (index !== -1) {
      // If upload exists, update it
      const newUploads = [...this.uploads];
      newUploads[index] = updatedUpload;
      this.uploads = newUploads;
    } else {
      // If it's a new upload, add it to the top of the list
      this.uploads = [updatedUpload, ...this.uploads];
      this.paginatorConfig = { ...this.paginatorConfig, length: this.uploads.length };
    }
    // Tell Angular to run change detection for this component
    this.cdr.markForCheck();
  }

  ngOnDestroy(): void {
    if (this.uploadsUpdateSubscription) {
      this.uploadsUpdateSubscription.unsubscribe();
    }
    // Optionally stop the connection if this is the last component using it
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      this.uploadForm.patchValue({
        fileName: file.name,
        fileContent: file
      });
    }
  }

  onSubmit(): void {
    if (this.uploadForm.valid) {
      this.isUploading = true;
      const formValue = this.uploadForm.getRawValue();

      // We use the non-null assertion operator (!) because Validators.required
      // guarantees the values are present if the form is valid.
      const uploadPayload: BookmarksBulkUpload = {
        FileName: formValue.fileName!,
        FileContent: formValue.fileContent!,
        UploadTimestamp: new Date().toISOString()
      };

      const formData = new FormData();
      formData.append('FileContent', uploadPayload.FileContent, uploadPayload.FileName);
      formData.append('FileName', uploadPayload.FileName);
      formData.append('UploadTimestamp', uploadPayload.UploadTimestamp);

      this.bookmarkService.uploadBookmarks(formData).subscribe({
        next: (response: UploadResponse) => {
          this.toastr.success(response.message);
          this.isUploading = false;
          this.uploadForm.reset();
          this.fileInput.nativeElement.value = '';
        },
        error: (error: HttpErrorResponse) => {
          // The error toast is now handled globally by the error-interceptor.
          // The component just needs to handle component-specific state.
          console.error('Upload failed:', error.error as ApiErrorResponse);
          this.isUploading = false;
        }
      });
    }
  }

  handleCustomAction(event: any): void {
    console.log('Custom action triggered for:', event);
    // Implement your custom action logic here
    
  }
}
import { Component } from '@angular/core';
import { PageHeader } from '@shared';
import { JOB_NOTIFICATIONS } from './data-temp';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
  imports: [PageHeader],
})
export class Dashboard {


  jobNotifications = JOB_NOTIFICATIONS;

  
}

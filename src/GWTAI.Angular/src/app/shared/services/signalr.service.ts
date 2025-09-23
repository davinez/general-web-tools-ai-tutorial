import { Injectable, Inject } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { API_URL } from '@core';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private hubConnection!: signalR.HubConnection;

  constructor(@Inject(API_URL) private apiUrl: string) {}

  public async startConnection(): Promise<void> {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.apiUrl}/jobEventStatusHub`)
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    try {
      await this.hubConnection.start();
      console.log('SignalR connection started successfully.');

      this.hubConnection.onclose(error => {
        console.error('SignalR connection closed.', error);
      });

      this.hubConnection.onreconnecting(error => {
        console.warn('SignalR is reconnecting...', error);
      });

      this.hubConnection.onreconnected(connectionId => {
        console.log(`SignalR reconnected with ID: ${connectionId}`);
      });

    } catch (err) {
      console.error('Error while starting SignalR connection: ', err);
    }
  }

  public getEventObservable<T>(eventName: string): Observable<T> {
    const subject = new Subject<T>();
    this.hubConnection.on(eventName, (data: T) => {
      console.log(`SignalR event '${eventName}' received with data:`, data);
      subject.next(data);
    });
    return subject.asObservable();
  }
}
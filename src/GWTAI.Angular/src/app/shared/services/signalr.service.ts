import { Injectable, inject } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HubConnection, HubConnectionState } from '@microsoft/signalr';
import { TokenService } from '@core';
import { Subject, Observable } from 'rxjs';

/*
A dedicated, reusable service to manage the SignalR connection 
and expose server events as RxJS Observables.

This service is the core of the real-time infrastructure. 
It doesn't know anything about jobs or tables; 
it only manages the connection and events.
*/
@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: HubConnection;
  // A dictionary to hold subjects for different server events
  private eventSubjects: { [eventName: string]: Subject<any> } = {};
  private tokenService = inject(TokenService);

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/jobEventStatusHub', {
        // Provide the JWT to authenticate the connection.
        // The backend uses this to associate the connection with a user.
       // accessTokenFactory: () => this.tokenService.getBearerToken().replace('Bearer ', ''),
      })
      .withAutomaticReconnect()
      .build();
  }

  // Starts the connection if not already connected
  public async startConnection(): Promise<void> {
    if (this.hubConnection.state === HubConnectionState.Disconnected) {
      try {
        await this.hubConnection.start();
        console.log('SignalR Connected.');
      } catch (err) {
        console.error('Error while starting SignalR connection: ' + err);
      }
    }
  }

  // Returns an observable for a specific server event, creating one if it doesn't exist
  public getEventObservable<T>(eventName: string): Observable<T> {
    if (!this.eventSubjects[eventName]) {
      this.eventSubjects[eventName] = new Subject<T>();
      
      // Register the handler on the hub connection
      this.hubConnection.on(eventName, (data: T) => {
        this.eventSubjects[eventName].next(data);
      });
    }
    return this.eventSubjects[eventName].asObservable();
  }
}
import * as signalR from '@microsoft/signalr';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  private hubConnection!: signalR.HubConnection;

  startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:5001/auctionHub')
      .build();

    this.hubConnection.start()
      .catch(err => console.log(err));
  }

  onBidUpdate(callback: (data: any) => void) {
    this.hubConnection.on('ReceiveBidUpdate', callback);
  }
}
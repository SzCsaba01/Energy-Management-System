import { Injectable } from "@angular/core";
import * as signalR from "@microsoft/signalr";
import { Guid } from "guid-typescript";
import { Observable } from "rxjs";


@Injectable({
    providedIn: 'root'
})
export class NotificationService {
    private hubConnection: signalR.HubConnection;

    constructor() {
        //http://localhost:5173/deviceNotificationHub
        //http://monitoring-and-communication-microservice/deviceNotificationHub
        this.hubConnection = 
            new signalR.HubConnectionBuilder()
            .withUrl('http://localhost:5173/deviceNotificationHub')
            .build();
    }

    public startConnection = () => {
        if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
            return;
        }
        this.hubConnection
            .start()
    }

    public stopConnection = () => {
        if (this.hubConnection.state === signalR.HubConnectionState.Disconnected) {
            return;
        }
        this.hubConnection
            .stop()
    }

    public joinGroup(deviceId: Guid) {
        if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
            this.hubConnection.invoke('JoinGroupAsync', deviceId)
        }
        else {
            setTimeout(() => this.joinGroup(deviceId), 500);
        }
    }

    public leaveGroup(deviceId: Guid) {
        if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
            this.hubConnection.invoke('LeaveGroupAsync', deviceId)
        }
        else {
            setTimeout(() => this.leaveGroup(deviceId), 500);
        }
    }

    public addNotificationListener(): Observable<string> {
        return new Observable<string>(observer => {
            this.hubConnection.on('ReceiveDeviceNotification', (data: string) => {
                observer.next(data);
            });
        });
    }
}
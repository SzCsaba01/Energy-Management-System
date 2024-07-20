import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Guid } from 'guid-typescript';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IChat } from '../models/chat/chat.model';
import { HttpClient } from '@angular/common/http';
import { IMessage } from '../models/chat/message.model';
import { ITyping } from '../models/chat/typing.model';


@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private _apiUrl: string;
  private hubConnection: signalR.HubConnection;

  constructor(private http: HttpClient) { 
    this._apiUrl = environment.chatApiUrl + 'Message';
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5031/messageHub')
      .build();
  }

  public sendMessage(message: IMessage): Observable<IMessage> {
    return this.http.post<IMessage>(`${this._apiUrl}/SendMessage`, message);
  }

  public getMessagesByAdminIdAndUsernames(adminId: Guid, usernames: string[]): Observable<IChat[]> {
    return this.http.post<IChat[]>(`${this._apiUrl}/GetMessagesByAdminIdAndUsernames?adminId=${adminId}`, usernames, {responseType: 'json'});
  }

  public getMessagesByAdminIdAndUsername(adminId: Guid, username: string): Observable<IChat> {
    return this.http.get<IChat>(`${this._apiUrl}/GetMessagesByAdminIdAndUsername?adminId=${adminId}&username=${username}`);
  }

  public updateMessage(message: IMessage[]): Observable<IMessage[]> {
    return this.http.put<IMessage[]>(`${this._apiUrl}/UpdateMessage`, message);
  }

  public typing(typing: ITyping): Observable<ITyping> {
    return this.http.put<ITyping>(`${this._apiUrl}/Typing`, typing);
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

  public joinGroup(adminId: Guid, username: string) {
    if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
      this.hubConnection.invoke('JoinGroupAsync', adminId, username)
    }
    else {
      setTimeout(() => this.joinGroup(adminId, username), 500);
    }
  }

  public leaveGroup(adminId: Guid, username: string) {
    if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
      this.hubConnection.invoke('LeaveGroupAsync', adminId, username)
    }
    else {
      setTimeout(() => this.leaveGroup(adminId, username), 500);
    }
  }

  public addMessageListener(): Observable<IMessage> {
    return new Observable<IMessage>(observer => {
      this.hubConnection.on('ReceiveMessage', (data: IMessage) => {
        observer.next(data);
      });
    });
  }

  public addTypingListener(): Observable<ITyping> {
    return new Observable<ITyping>(observer => {
      this.hubConnection.on('ReceiveTyping', (data: ITyping) => {
        observer.next(data);
      });
    });
  }

  public addUpdateMessageListener(): Observable<IMessage[]> {
    return new Observable<IMessage[]>(observer => {
      this.hubConnection.on('ReceiveUpdateMessage', (data: IMessage[]) => {
        observer.next(data);
      });
    });
  }
}

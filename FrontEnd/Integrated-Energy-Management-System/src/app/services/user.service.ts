import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { Observable } from "rxjs";
import { IUser } from "../models/user/user.model";
import { IDeviceToUser } from "../models/user-device/device-to-user.model";
import { IUserWithDevices } from "../models/user/user-with-devices.model";
import { Guid } from "guid-typescript";

@Injectable({
    providedIn: 'root'
})
export class UserService {
    private _apiUrl: string;

    constructor(private http: HttpClient) {
        this._apiUrl = environment.userApiUrl + 'User';
    }

    public addUser(addUser: IUser) {
        return this.http.post(`${this._apiUrl}/AddUser`, addUser);
    }

    public getAdminId(): Observable<Guid> {
        return this.http.get<Guid>(`${this._apiUrl}/GetAdminId`);
    }
    
    public getUsernames(): Observable<string[]> {
        return this.http.get<string[]>(`${this._apiUrl}/GetUsernames`);
    }

    public getAllUsers(): Observable<IUserWithDevices[]>{
        return this.http.get<IUserWithDevices[]>(`${this._apiUrl}/GetAllUsers`);
    }

    public updateUser(updateUser: IUser) {
        return this.http.put(`${this._apiUrl}/UpdateUser`, updateUser);
    }

    public assignDeviceToUser(deviceToUser: IDeviceToUser) {
        return this.http.put(`${this._apiUrl}/AssignDeviceToUser`, deviceToUser);
    };

    public removeDeviceFromUser(deviceToUser: IDeviceToUser) {
        return this.http.put(`${this._apiUrl}/RemoveDeviceFromUserByUserIdAndDeviceId`, deviceToUser);
    }

    public deleteUser(username: string) {
        return this.http.delete(`${this._apiUrl}/DeleteUser?username=${username}`);
    }

}
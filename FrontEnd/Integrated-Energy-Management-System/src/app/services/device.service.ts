import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { IDevice } from "../models/device/device.model";
import { Observable } from "rxjs";
import { Guid } from "guid-typescript";
  

@Injectable({
    providedIn: 'root'
})
export class DeviceService {
    private _apiUrl: string;

    constructor(private http: HttpClient) {
        this._apiUrl = environment.deviceApiUrl + 'Device';
    }

    public addDevice(addDevice: IDevice): Observable<IDevice> {
        return this.http.post<IDevice>(`${this._apiUrl}/AddDevice`, addDevice);
    }

    public getAllDevices(): Observable<IDevice[]> {
        return this.http.get<IDevice[]>(`${this._apiUrl}/GetAllDevices`);
    }

    public getDevicesByUserId(userId: Guid): Observable<IDevice[]> {
        return this.http.get<IDevice[]>(`${this._apiUrl}/GetDevicesByUserId?userId=${userId}`);
    }

    public getUnassignedDevices(): Observable<IDevice[]> {
        return this.http.get<IDevice[]>(`${this._apiUrl}/GetUnassignedDevices`);
    }

    public updateDevice(device: IDevice) {
        return this.http.put(`${this._apiUrl}/UpdateDevice`, device);
    }

    public deleteDeviceById(deviceId: Guid) {
        return this.http.delete(`${this._apiUrl}/DeleteDeviceById?id=${deviceId}`);
    }
}
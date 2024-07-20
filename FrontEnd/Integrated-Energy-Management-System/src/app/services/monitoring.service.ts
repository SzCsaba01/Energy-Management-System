import { Injectable } from "@angular/core";
import { Guid } from "guid-typescript";
import { Observable } from "rxjs";
import { IMonitoring } from "../models/monitoring/monitoring.model";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";

@Injectable({
    providedIn: 'root'
})
export class MonitoringService {
    private _apiUrl: string;
    constructor(private http: HttpClient) {
        this._apiUrl = environment.monitoringApiUrl + 'Monitoring';
    }

    public getMonitoingsByDeviceIds(deviceIds: Guid[]) : Observable<IMonitoring[]> {
        return this.http.post<IMonitoring[]>(`${this._apiUrl}/GetMonitoringsByDeviceIds`, deviceIds);
    }
}
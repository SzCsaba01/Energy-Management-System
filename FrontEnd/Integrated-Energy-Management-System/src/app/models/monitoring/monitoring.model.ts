import { Guid } from "guid-typescript";

export interface IMonitoring {
    deviceId: Guid;
    deviceName: string;
    measurmentValue: number;
    timestamp: Date;
}
import { Guid } from "guid-typescript";

export interface IDeviceToUser {
    username: string;
    deviceId: Guid;
}
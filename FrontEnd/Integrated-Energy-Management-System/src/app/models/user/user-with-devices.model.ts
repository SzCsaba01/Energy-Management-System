import { IDevice } from "../device/device.model";

export interface IUserWithDevices {
    email: string;
    username: string;
    firstName: string;
    lastName: string;
    password: string;
    role: string;
    devices: IDevice[];
}
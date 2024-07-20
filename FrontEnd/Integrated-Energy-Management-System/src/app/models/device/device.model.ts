import { Guid } from "guid-typescript";

export interface IDevice {
    id: Guid;
    name: string;
    description: string;
    address: string;
    maxHourlyEnergyConsumption: number;
}
import { Guid } from "guid-typescript";

export interface ITyping {
    adminId: Guid;
    username: string;
    isAdminTyping: boolean;
    isUserTyping: boolean;
}
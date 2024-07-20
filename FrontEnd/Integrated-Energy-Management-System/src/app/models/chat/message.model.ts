import { Guid } from "guid-typescript";

export interface IMessage {
    id: Guid;
    adminId: Guid;
    username: string;
    message: string;
    isAdminSeen: boolean;
    isUserSeen: boolean;
    sentAt: Date;
}
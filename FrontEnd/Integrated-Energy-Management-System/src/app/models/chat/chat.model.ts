import { IMessage } from "./message.model";

export interface IChat {
    username: string;
    messages: IMessage[];
    isLastMessageSeenByAdmin: boolean;
    isLastMessageSeenByUser: boolean;
}
import { Component, OnDestroy, OnInit } from '@angular/core';
import { el } from 'date-fns/locale';
import { Guid } from 'guid-typescript';
import { takeUntil } from 'rxjs';
import { RoleChecker } from 'src/app/helpers/role-checker';
import { IChat } from 'src/app/models/chat/chat.model';
import { IMessage } from 'src/app/models/chat/message.model';
import { ITyping } from 'src/app/models/chat/typing.model';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChatService } from 'src/app/services/chat.service';
import { UserService } from 'src/app/services/user.service';
import { SelfUnsubscriberBase } from 'src/app/utils/SelfUnsubscriberBase';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent extends SelfUnsubscriberBase implements OnInit, OnDestroy {
  usernames: string[] | undefined;
  chats: IChat [] = [];
  loggedUserRole: string = {} as string;
  selectedChat: IChat | undefined;

  typeing: ITyping = {} as ITyping;

  newMessage: IMessage = {} as IMessage;

  adminId: Guid | undefined;
  loggedUserId: Guid | undefined;
  loggedUsername: string | undefined;

  public constructor(
    private userService: UserService,
    private chatService: ChatService,
    private authenticationService: AuthenticationService,
    public roleChecker: RoleChecker,
  ){
    super();
    this.typeing.isAdminTyping = false;
    this.typeing.isUserTyping = false;
  }  

  ngOnInit(): void {
    this.loggedUserRole = this.authenticationService.getUserRole()!;
    this.loggedUserId = this.authenticationService.getUserId()! as unknown as Guid
    this.loggedUsername = this.authenticationService.getUsername()!;

    if (this.roleChecker.isAdmin(this.loggedUserRole)){
      this.newMessage.adminId = this.loggedUserId!;
    } else if (this.roleChecker.isUser(this.loggedUserRole)) {
      this.newMessage.username = this.loggedUsername!;
    }

    this.getChats();
  }

  showChatMessages(chat: IChat): void {
    this.newMessage.message = '';
    this.selectedChat = chat;

    this.typeing.isAdminTyping = false;
    this.typeing.isUserTyping = false;

    if (this.roleChecker.isAdmin(this.loggedUserRole)) {
      this.typeing.adminId = this.loggedUserId!;
      this.typeing.username = this.selectedChat.username;
      this.newMessage.username = this.selectedChat.username;

      if (this.selectedChat.messages[this.selectedChat.messages.length - 1].isAdminSeen == false) {
        const messagesToBeUpdated: IMessage[] = [];
        this.selectedChat.messages.forEach((message: IMessage) => {
          if (message.isAdminSeen == false){
            message.isAdminSeen = true;
            messagesToBeUpdated.push(message);
          }
        });

        this.chatService.updateMessage(messagesToBeUpdated)
          .pipe(takeUntil(this.ngUnsubscribe))
          .subscribe();

        this.chats.forEach((chat: IChat) => {
          if (chat.username == this.selectedChat!.username){
            chat.isLastMessageSeenByAdmin = true;
          }
        });
      }
    } else if (this.roleChecker.isUser(this.loggedUserRole)) {
      this.typeing.adminId = this.adminId!;
      this.typeing.username = this.loggedUsername!;

      if (this.selectedChat.messages[this.selectedChat.messages.length - 1].isUserSeen == false) {
        const messagesToBeUpdated: IMessage[] = [];
        this.selectedChat.messages.forEach((message: IMessage) => {
          if (message.isUserSeen == false){
            message.isUserSeen = true;
            messagesToBeUpdated.push(message);
          }
        });

        this.chatService.updateMessage(messagesToBeUpdated)
          .pipe(takeUntil(this.ngUnsubscribe))
          .subscribe();

        this.chats.forEach((chat: IChat) => {
          if (chat.username == this.selectedChat!.username){
            chat.isLastMessageSeenByUser = true;
          }
        });
      }
    }
  }

  private getChats(): void {
    if (this.roleChecker.isAdmin(this.loggedUserRole)){
      this.userService.getUsernames()
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe((usernames: string[]) => {
        this.usernames = usernames;

        this.connectToChatHub();

        this.chatService.getMessagesByAdminIdAndUsernames(this.loggedUserId!, this.usernames!)
          .pipe(takeUntil(this.ngUnsubscribe))
          .subscribe((chats: IChat[]) => {
            this.chats = chats;

            if (this.chats.length == 0){
              return;
            }
            
            this.chats.forEach((chat: IChat) => {
              chat.isLastMessageSeenByAdmin = this.isLastMessageSeenByAdmin(chat);
              chat.isLastMessageSeenByUser = this.isLastMessageSeenByUser(chat);
            });
          });

      });
    } else if (this.roleChecker.isUser(this.loggedUserRole)) {
      this.userService.getAdminId()
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe((adminId: Guid) => {
          this.adminId = adminId;
          this.newMessage.adminId = this.adminId!;

          this.connectToChatHub();
                
          this.chatService.getMessagesByAdminIdAndUsername(this.adminId!, this.loggedUsername!)
          .pipe(takeUntil(this.ngUnsubscribe))
          .subscribe((chats: IChat) => {
            this.chats = [chats];
            this.chats[0].isLastMessageSeenByAdmin = this.isLastMessageSeenByAdmin(this.chats[0]);
            this.chats[0].isLastMessageSeenByUser = this.isLastMessageSeenByUser(this.chats[0]);
          });
        });
    }
  }

  private isLastMessageSeenByAdmin(chat: IChat): boolean {
    if (this.roleChecker.isAdmin(this.loggedUserRole)){
      return chat.messages.length == 0 || chat.messages[chat.messages.length - 1].isAdminSeen;
    }
    return false;
  }
  private isLastMessageSeenByUser(chat: IChat): boolean {
    if (this.roleChecker.isUser(this.loggedUserRole)){
      return chat.messages.length == 0 || chat.messages[chat.messages.length - 1].isUserSeen;
    }
    return false;
  }

  private connectToChatHub(): void {
    this.chatService.startConnection();

    if (this.roleChecker.isAdmin(this.loggedUserRole)){
      this.usernames!.forEach((username: string) => {
        this.chatService.joinGroup(this.loggedUserId!, username);
      });
    }

    if (this.roleChecker.isUser(this.loggedUserRole)) {
      this.chatService.joinGroup(this.adminId!, this.loggedUsername!);
    }

    this.chatService.addMessageListener()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(message => {
        if (this.roleChecker.isAdmin(this.loggedUserRole)){
          this.chats.forEach((chat: IChat) => {
            if (chat.username == message.username) {
              chat.messages.push(message);

              if (this.selectedChat?.username !== chat.username){
                chat.isLastMessageSeenByAdmin = this.isLastMessageSeenByAdmin(chat);
              } else if (this.selectedChat?.username == chat.username){
                if (this.authenticationService.isAuthenticated() && message.isAdminSeen == false){
                  message.isAdminSeen = true;
                  this.chatService.updateMessage([message])
                    .pipe(takeUntil(this.ngUnsubscribe))
                    .subscribe();
                }
              }
            }
          });
        } else if (this.roleChecker.isUser(this.loggedUserRole)) {
          if (this.authenticationService.isAuthenticated() && message.isUserSeen == false) {
            message.isUserSeen = true;
            this.chatService.updateMessage([message])
              .pipe(takeUntil(this.ngUnsubscribe))
              .subscribe();
          }
          this.chats[0].messages.push(message);
        }
      });

    this.chatService.addUpdateMessageListener()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(messages => {
        if (this.roleChecker.isAdmin(this.loggedUserRole)) {
          this.chats.forEach((chat: IChat) => {
            if (chat.username == messages[0].username) {
              chat.messages = chat.messages.map((x) => {
                messages.forEach((updatedMessage: IMessage) => {
                  if (x.id == updatedMessage.id) {
                    x = updatedMessage;
                  }
                });
                return x;
              })
            }
          });
        } else if (this.roleChecker.isUser(this.loggedUserRole)) {
          this.chats[0].messages = this.chats[0].messages.map((x) => {
            messages.forEach((updatedMessage: IMessage) => {
              if (x.id == updatedMessage.id) {
                x = updatedMessage;
              }
            });
            return x;
          })
        }
      });

    this.chatService.addTypingListener()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(typing => {
        if (this.typeing.username == typing.username) {
          this.typeing = typing;
        }
      });
  }

  onTypingChange(): void {
    if (this.roleChecker.isAdmin(this.loggedUserRole)){
      if (this.typeing.isAdminTyping == true && this.newMessage.message == ''){
        this.typeing.isAdminTyping = false;
      }
      else if (this.typeing.isAdminTyping == false && this.newMessage.message != ''){
        this.typeing.isAdminTyping = true;
      } else {
        return;
      }
      this.chatService.typing(this.typeing)
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe();
    } else if (this.roleChecker.isUser(this.loggedUserRole)) {
      if (this.typeing.isUserTyping == true && this.newMessage.message == ''){
        this.typeing.isUserTyping = false;
      }
      else if (this.typeing.isUserTyping == false && this.newMessage.message != ''){
        this.typeing.isUserTyping = true;
      } else {
        return;
      }
      this.chatService.typing(this.typeing)
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe();
    }
  }

  sendMessage(): void {
    this.newMessage.message = this.newMessage.message.trim();
    
    if (this.newMessage.message == ''){
      return;
    }

    if (this.roleChecker.isAdmin(this.loggedUserRole)){
      this.newMessage.isAdminSeen = true;
      this.newMessage.isUserSeen = false;
    } else if (this.roleChecker.isUser(this.loggedUserRole)) {
      this.newMessage.isAdminSeen = false;
      this.newMessage.isUserSeen = true;
    }

    this.chatService.sendMessage(this.newMessage)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(() => {
        if (this.roleChecker.isAdmin(this.loggedUserRole)){
          this.typeing.isAdminTyping = false;
        } else if (this.roleChecker.isUser(this.loggedUserRole)) {
          this.typeing.isUserTyping = false;
        }

        this.newMessage.message = '';

        this.chatService.typing(this.typeing)
          .pipe(takeUntil(this.ngUnsubscribe))
          .subscribe();
      });
  }

  OnDestroy(): void {
    this.chatService.stopConnection();
    if (this.roleChecker.isAdmin(this.loggedUserRole)){
      this.usernames!.forEach((username: string) => {
        this.chatService.leaveGroup(this.loggedUserId!, username);
      });
    }

    if (this.roleChecker.isUser(this.loggedUserRole)) {
      this.chatService.leaveGroup(this.adminId!, this.loggedUsername!);
    }
  }
  
}

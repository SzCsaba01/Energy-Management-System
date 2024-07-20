import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Guid } from 'guid-typescript';
import { Subscription, take, takeUntil } from 'rxjs';
import { RoleChecker } from 'src/app/helpers/role-checker';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { DeviceService } from 'src/app/services/device.service';
import { NotificationService } from 'src/app/services/notification.service';
import { SelfUnsubscriberBase } from 'src/app/utils/SelfUnsubscriberBase';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent extends SelfUnsubscriberBase implements OnInit, OnDestroy {
  private authenticationSubscription: Subscription = {} as Subscription;
  private loginSubscription: Subscription = {} as Subscription;
  private deviceIds: Guid[] = [];

  notificationMessages: string[] = [];

  constructor (
    private authenticationService: AuthenticationService,
    private roleChecker: RoleChecker,
    private router: Router,
    private notificationService: NotificationService,
    private deviceService: DeviceService,
    ){
      super();
    }

  private loggedUserRole: string | null = '';

  ngOnInit(): void {
    this.loggedUserRole = this.authenticationService.getUserRole();
    if (this.loggedUserRole == null || this.roleChecker.isAdmin(this.loggedUserRole)) {
      return;
    }
    this.connectToDeviceHub();
  }

  isAdmin(): boolean {
    return this.loggedUserRole != null && this.roleChecker.isAdmin(this.loggedUserRole);
  }

  onLogout(): void {
    this.notificationService.stopConnection();
    this.deviceIds.forEach((id) => {
      this.notificationService.leaveGroup(id);
    });
    this.authenticationService.logout();
  }

  private connectToDeviceHub(): void {
    this.notificationService.startConnection();

    const userId =  this.authenticationService.getUserId()! as unknown as Guid

    if (userId != null) {
      this.deviceService.getDevicesByUserId(userId)
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe(devices => {
          devices.forEach(device => {
            this.deviceIds.push(device.id);
            this.notificationService.joinGroup(device.id);
          });
        });
    }

    this.notificationService.addNotificationListener()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(notification => {
        this.showNotification(notification);
      });
    
  }

  private showNotification(message: string): void {
    this.notificationMessages.push(message);
    const timeoutId = setTimeout(() => {
      const index = this.notificationMessages.indexOf(message);
      if (index !== -1) {
        this.notificationMessages.splice(index, 1);
      }
    }, 5000);
  
    this.ngUnsubscribe.pipe(take(1)).subscribe(() => clearTimeout(timeoutId));
  }

  OnDestroy(): void {
    this.notificationService.stopConnection();
    this.deviceIds.forEach(id => {
      this.notificationService.leaveGroup(id);
    });
  }

}

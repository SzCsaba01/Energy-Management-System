import { Component, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { takeUntil } from 'rxjs';
import { UserService } from 'src/app/services/user.service';
import { SelfUnsubscriberBase } from 'src/app/utils/SelfUnsubscriberBase';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { passwordFormat } from 'src/app/Formats/Formats';
import { IUserWithDevices } from 'src/app/models/user/user-with-devices.model';
import { IDevice } from 'src/app/models/device/device.model';
import { DeviceService } from 'src/app/services/device.service';
import { IDeviceToUser } from "src/app/models/user-device/device-to-user.model";
import { roles } from 'src/app/constants/roles';


@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent extends SelfUnsubscriberBase implements OnInit{
  @ViewChildren(MatPaginator) paginator = new QueryList<MatPaginator>();
  @ViewChildren(MatSort) sort = new QueryList<MatSort>();

  private readonly userRole = roles.user;
  
  dataSource = new MatTableDataSource<IUserWithDevices>();
  dataSourceSelectedUserDevices = new MatTableDataSource<IDevice>();
  unAssignedDevices: IDevice[] = [] as IDevice[];

  selectedUser: IUserWithDevices | undefined;
  selectedDevice: IDevice | undefined;

  isAddUserModalShown = false;
  isEditUserModalShown = false;
  isEditUserDevicesShown = false;

  addUserFG: FormGroup;
  editUserFG: FormGroup;

  editEmailFC: FormControl;
  editUsernameFC: FormControl;
  editFirstNameFC: FormControl;
  editLastNameFC: FormControl;
  editPasswordFC: FormControl;
  editRoleFC: FormControl = new FormControl(this.userRole);

  addEmailFC: FormControl;
  addUsernameFC: FormControl;
  addFirstNameFC: FormControl;
  addLastNameFC: FormControl;
  addPasswordFC: FormControl;
  addRoleFC: FormControl = new FormControl(this.userRole);

  displayedUserColumns: string[] = ['email', 'username', 'firstName', 'lastName', 'role'];
  displayedDeviceColumns: string[] = ['name', 'description', 'address', 'maxHourlyEnergyConsumption', 'deleteButton'];

  constructor(
    private userService: UserService,
    private deviceService: DeviceService,
  ) {
    super();

    this.addEmailFC = new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(50), Validators.email]);
    this.addUsernameFC = new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(50)]);
    this.addFirstNameFC = new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(50)]);
    this.addLastNameFC = new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(50)]);
    this.addPasswordFC = new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(50), Validators.pattern(passwordFormat)]);

    this.editEmailFC = new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(50), Validators.email]);
    this.editUsernameFC = new FormControl('');
    this.editFirstNameFC = new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(50)]);
    this.editLastNameFC = new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(50)]);
    this.editPasswordFC = new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(50), Validators.pattern(passwordFormat)]);

    this.addUserFG = new FormGroup({
      'email': this.addEmailFC,
      'username': this.addUsernameFC,
      'firstName': this.addFirstNameFC,
      'lastName': this.addLastNameFC,
      'password': this.addPasswordFC,
      'role': this.addRoleFC,
    });

    this.editUserFG = new FormGroup({
      'email': this.editEmailFC,
      'username': this.editUsernameFC,
      'firstName': this.editFirstNameFC,
      'lastName': this.editLastNameFC,
      'password': this.editPasswordFC,
      'role': this.editRoleFC,
    });
  }

  ngOnInit(): void {
    this.userService.getAllUsers()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((users: IUserWithDevices[]) => {
        this.dataSource.data = users;
        this.dataSource.paginator = this.paginator.toArray()[0];
        this.dataSource.sort = this.sort.toArray()[0];
      });
    
    this.deviceService.getUnassignedDevices()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((devices: IDevice[]) => {
        this.unAssignedDevices = devices;
      });
    
    this.dataSourceSelectedUserDevices.paginator = this.paginator.toArray()[1];
    this.dataSourceSelectedUserDevices.sort = this.sort.toArray()[1];
  }

  changeShowAddUserModal(): void {
    this.isAddUserModalShown = ! this.isAddUserModalShown;
  }

  onAddUser(user: IUserWithDevices) {
    this.userService.addUser(user)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(() => {
        this.dataSource.data.push(user);
        this.dataSource._updateChangeSubscription();
        this.isAddUserModalShown = false;
        this.addUserFG.reset();
        this.addRoleFC.setValue(this.userRole);
      });
  }

  exitEditUser(): void {
    this.isEditUserModalShown = false;
    this.isEditUserDevicesShown = false;
    this.selectedUser = undefined;
  }

  onRowClicked(user: IUserWithDevices): void {
    this.selectedUser = user;
    this.isEditUserModalShown = true;
    this.editUserFG.patchValue(user);
    this.dataSourceSelectedUserDevices.data = user.devices;
  }

  onSaveChanges(user: IUserWithDevices): void {
    this.userService.updateUser(user)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(() => {
        user.devices = this.selectedUser!.devices;
        this.dataSource.data = this.dataSource.data.map(u => u.username === user.username ? user : u);
        this.dataSource._updateChangeSubscription();
      });
  }

  onChangeShowEditUserDevices(): void {
    this.isEditUserDevicesShown = ! this.isEditUserDevicesShown;
  }

  onDeleteSelectedUser(): void {
    if (!this.selectedUser) {
      return;
    }

    this.userService.deleteUser(this.selectedUser.username)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(() => {
        this.dataSource.data = this.dataSource.data.filter(u => u.username !== this.selectedUser!.username);
        this.dataSource._updateChangeSubscription();
      });
  }

  onAssignDeviceToSelectedUser(): void {
    if (this.selectedUser && this.selectedDevice) {
      const deviceToUser: IDeviceToUser = {
        username: this.selectedUser.username,
        deviceId: this.selectedDevice.id
      };
      this.userService.assignDeviceToUser(deviceToUser)
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe(() => {
          this.selectedUser!.devices.push(this.selectedDevice!);
          this.unAssignedDevices = this.unAssignedDevices.filter(d => d.id !== this.selectedDevice!.id);
          this.selectedDevice = undefined;
          this.dataSourceSelectedUserDevices.data = this.selectedUser!.devices;
          this.dataSourceSelectedUserDevices._updateChangeSubscription();
        });
    }
  }

  onRemoveDeviceFromSelectedUser(device: IDevice): void {
    if (this.selectedUser) {
      const deviceToUser: IDeviceToUser = {
        username: this.selectedUser.username,
        deviceId: device.id
      };
      this.userService.removeDeviceFromUser(deviceToUser)
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe(() => {
          this.selectedUser!.devices = this.selectedUser!.devices.filter(d => d.id !== device.id);
          this.unAssignedDevices.push(device);
          this.dataSourceSelectedUserDevices.data = this.selectedUser!.devices;
          this.dataSourceSelectedUserDevices._updateChangeSubscription();
        });
    }
  }
}

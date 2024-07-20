import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Guid } from 'guid-typescript';
import { takeUntil } from 'rxjs';
import { roles } from 'src/app/constants/roles';
import { IDevice } from 'src/app/models/device/device.model';
import { IMonitoring } from 'src/app/models/monitoring/monitoring.model';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { DeviceService } from 'src/app/services/device.service';
import { MonitoringService } from 'src/app/services/monitoring.service';
import { SelfUnsubscriberBase } from 'src/app/utils/SelfUnsubscriberBase';
import { Chart } from 'chart.js';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';

@Component({
  selector: 'app-devices',
  templateUrl: './devices.component.html',
  styleUrls: ['./devices.component.scss']
})
export class DevicesComponent extends SelfUnsubscriberBase implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator | undefined;
  @ViewChild(MatSort) sort: MatSort | undefined;

  userRole = '';

  private selectedDeviceId: any;
  dataSource = new MatTableDataSource<IDevice>();

  monitoringData: IMonitoring[] = [];

  isAddDeviceModalShown = false;
  isEditDeviceModalShown = false;
  addDeviceFG: FormGroup;
  editDeviceFG: FormGroup;

  editNameFC: FormControl;
  editDescriptionFC: FormControl;
  editAddressFC: FormControl;
  editMaxHourlyEnergyConsumptionFC: FormControl;

  addNameFC: FormControl;
  addDescriptionFC: FormControl;
  addAddressFC: FormControl;
  addMaxHourlyEnergyConsumptionFC: FormControl;

  displayedColumns: string[] = ['name', 'description', 'address', 'maxHourlyEnergyConsumption'];

  monitoringChartConfig: any;
  selectedDate: Date = new Date();

  constructor (
    private deviceService: DeviceService,
    private authenticationService: AuthenticationService,
    private monitoringService: MonitoringService
  ) {
    super();

    this.addNameFC = new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(50)]);
    this.addDescriptionFC = new FormControl('', [Validators.required, Validators.minLength(10), Validators.maxLength(200)]);
    this.addAddressFC = new FormControl('', [Validators.required, Validators.minLength(10), Validators.maxLength(100)]);
    this.addMaxHourlyEnergyConsumptionFC = new FormControl('', [Validators.required, Validators.min(5), Validators.max(50)]);

    this.editNameFC = new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(50)]);
    this.editDescriptionFC = new FormControl('', [Validators.required, Validators.minLength(10), Validators.maxLength(200)]);
    this.editAddressFC = new FormControl('', [Validators.required, Validators.minLength(10), Validators.maxLength(100)]);
    this.editMaxHourlyEnergyConsumptionFC = new FormControl('', [Validators.required, Validators.min(5), Validators.max(50)]);

    this.addDeviceFG = new FormGroup({
      'name': this.addNameFC,
      'description': this.addDescriptionFC,
      'address': this.addAddressFC,
      'maxHourlyEnergyConsumption': this.addMaxHourlyEnergyConsumptionFC
    });

    this.editDeviceFG = new FormGroup ({
      'name': this.editNameFC,
      'description': this.editDescriptionFC,
      'address': this.editAddressFC,
      'maxHourlyEnergyConsumption': this.editMaxHourlyEnergyConsumptionFC
    })
  }

  ngOnInit(): void {
    this.userRole = this.authenticationService.getUserRole()!;
    if (this.userRole == roles.user) {
      this.deviceService.getDevicesByUserId(this.authenticationService.getUserId()! as unknown as Guid)
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe((result) => {
        this.dataSource = new MatTableDataSource<IDevice>(result);
        this.dataSource.paginator = this.paginator!;
        this.dataSource.sort = this.sort!;

        const deviceIds = result.map(x => {return x.id});
        this.monitoringService.getMonitoingsByDeviceIds(deviceIds)
          .pipe(takeUntil(this.ngUnsubscribe))
          .subscribe((result) => {
            this.monitoringData = result;
            this.initializeLineChart();
          });
      });
    } else {
      this.deviceService.getAllDevices()
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe((result) => {
        this.dataSource = new MatTableDataSource<IDevice>(result);
        this.dataSource.paginator = this.paginator!;
        this.dataSource.sort = this.sort!;
      })
    }
  }

  private initializeLineChart(): void {
    const ctx = document.getElementById('monitoringChart') as HTMLCanvasElement;
    const filteredData = this.monitoringData.filter(
      (data) =>
        new Date(data.timestamp).toDateString() ===
        this.selectedDate.toDateString()
    );
    const labels = filteredData.map((data) => new Date(data.timestamp));
    const values = filteredData.map((data) => data.measurmentValue);
    const deviceNames = filteredData.map((data) => data.deviceName); 

    this.monitoringChartConfig = new Chart(ctx, {
      type: 'line',
      data: {
        labels: labels,
        datasets: [
          {
            label: 'Energy consumption',
            data: values,
            borderColor: '#3cba9f',
            fill: false,
          },
        ],
      },
      options: {
        scales: {
          x: {
            type: 'time',
            time: {
              unit: 'hour',
            },
          },
          y: {
            beginAtZero: true,
          },
        },
        plugins: {
          tooltip: {
            callbacks: {
              title: function (tooltipItems) {
                return 'Device: ' + deviceNames[tooltipItems[0].dataIndex];
              },
            },
          },
        },
      },
    });
  }

  dateChanged() {
    this.updateChart();
  }

  updateChart() {
    const filteredData = this.monitoringData.filter(
      (data) =>
        new Date(data.timestamp).toDateString() ===
        this.selectedDate.toDateString()
    );

    const labels = filteredData.map((data) => new Date(data.timestamp));
    const values = filteredData.map((data) => data.measurmentValue);
    const deviceNames = filteredData.map((data) => data.deviceName); 

    this.monitoringChartConfig.data.labels = labels;
    this.monitoringChartConfig.data.datasets[0].data = values;

    this.monitoringChartConfig.options.plugins.tooltip.callbacks = {
      title: function (tooltipItems: any) {
        return 'Device: ' + deviceNames[tooltipItems[0].dataIndex];
      },
    };
    this.monitoringChartConfig.update();
  }

  changeShowAddDeviceModal(): void {
    this.isAddDeviceModalShown = ! this.isAddDeviceModalShown;
  }

  onAddDevice(device: IDevice) {
    this.deviceService.addDevice(device)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((result) => {
        this.addDeviceFG.reset();
        this.isAddDeviceModalShown = false;
        this.dataSource.data.push(result);
        this.dataSource._updateChangeSubscription();
      })
  }

  exitEditDevice() {
    this.isEditDeviceModalShown = false;
    this.editDeviceFG.reset();
  }

  onRowClicked(device: IDevice): void {
    if (this.userRole == roles.admin) {
      this.selectedDeviceId = device.id;

      this.editNameFC.setValue(device.name);
      this.editDescriptionFC.setValue(device.description);
      this.editAddressFC.setValue(device.address);
      this.editMaxHourlyEnergyConsumptionFC.setValue(device.maxHourlyEnergyConsumption);

      this.isEditDeviceModalShown = true;
    }
  }

  onSaveChanges(device: IDevice): void {
    device.id = this.selectedDeviceId;
    this.deviceService.updateDevice(device)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(() => {
      const index = this.dataSource.data.findIndex(x => {return x.id == device.id});
      this.dataSource.data[index] = device;
      this.dataSource._updateChangeSubscription();
      this.isEditDeviceModalShown = false;
    })
  }

  onDeleteSelectedDevice(): void {
    this.deviceService.deleteDeviceById(this.selectedDeviceId)
      .pipe(takeUntil(this.ngUnsubscribe))  
      .subscribe(() => {
        const index = this.dataSource.data.findIndex(x => {return x.id == this.selectedDeviceId});
        this.dataSource.data.splice(index, 1);
        this.dataSource._updateChangeSubscription();
        this.isEditDeviceModalShown = false;
        this.editAddressFC.reset();
      })
  }

}

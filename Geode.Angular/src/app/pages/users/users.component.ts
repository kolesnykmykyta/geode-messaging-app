import { Component, OnInit } from '@angular/core';
import { ColDef } from 'ag-grid-community';
import { UsersService } from '../../shared/services/users.service';
import { UserInfo } from '../../shared/interfaces/user-info.interface';
import { Filter } from '../../shared/interfaces/filter.interface';
import { CountryNumberPipe } from '../../shared/pipes/country-number.pipe';
import { DateFormatterPipe } from '../../shared/pipes/date-formatter.pipe';
import {
  USERS_FILTER_PERMISSION,
  USERS_READ_PERMISSION,
} from '../../shared/constants/permissions.constants';

@Component({
  selector: 'gd-users',
  templateUrl: './users.component.html',
  styleUrl: './users.component.css',
})
export class UsersComponent implements OnInit {
  readonly USERS_READ_PERMISSION = USERS_READ_PERMISSION;
  readonly USERS_FILTER_PERMISSION = USERS_FILTER_PERMISSION;

  properties: string[] = ['UserName', 'Email', 'PhoneNumber'];
  rowData: UserInfo[] = [];
  colDefs: ColDef[] = [
    { headerName: 'Username', field: 'userName' },
    { headerName: 'Email', field: 'email' },
    {
      headerName: 'Balance',
      field: 'balance',
      valueFormatter: (params) =>
        new CountryNumberPipe().transform(params.value, 'de-DE'),
    },
    {
      headerName: 'Birth Date',
      field: 'birthDate',
      valueFormatter: (params) =>
        new DateFormatterPipe().transform(new Date(params.value), 'es-ES'),
    },
    {
      headerName: 'Phone',
      field: 'phoneNumber',
      cellRenderer: this.phoneCellRenderer.bind(this),
    },
    { headerName: '', cellRenderer: this.videoCallCellRenderer },
  ];
  isLoading: boolean = false;

  constructor(private usersService: UsersService) {}

  ngOnInit(): void {
    this.updateRowData();
  }

  updateRowData(filter: Filter | null = null): void {
    this.isLoading = true;
    this.usersService
      .getAllUsers(filter)
      .subscribe((result) => (this.rowData = result))
      .add(() => (this.isLoading = false));
  }

  private videoCallCellRenderer(params: any): string {
    const link = `<a href="">Video Call</a>`;
    return link;
  }

  private phoneCellRenderer(params: any): string {
    const phoneNumber = params.value;
    return `<a style="color: blue; text-decoration: underline; cursor: pointer;" 
            href="tel:${phoneNumber}">${phoneNumber ?? ''}</a>`;
  }
}

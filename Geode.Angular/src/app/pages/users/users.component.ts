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
import { COUNTRY_CODES } from '../../shared/constants/country-codes.constant';
import { FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'gd-users',
  templateUrl: './users.component.html',
  styleUrl: './users.component.css',
  providers: [CountryNumberPipe, DateFormatterPipe],
})
export class UsersComponent {
  readonly USERS_READ_PERMISSION = USERS_READ_PERMISSION;
  readonly USERS_FILTER_PERMISSION = USERS_FILTER_PERMISSION;
  readonly CountryCodes = COUNTRY_CODES;

  selectedLocale: FormControl = new FormControl(this.CountryCodes[0].code);

  properties: string[] = ['UserName', 'Email', 'PhoneNumber'];
  rowData: UserInfo[] = this.activatedRoute.snapshot.data['initData'];
  colDefs: ColDef[] = [];
  isLoading: boolean = false;

  constructor(
    private usersService: UsersService,
    private countryNumber: CountryNumberPipe,
    private dateFormatter: DateFormatterPipe,
    private activatedRoute: ActivatedRoute
  ) {}

  updateRowData(filter: Filter | null = null): void {
    this.isLoading = true;
    this.usersService
      .getAllUsers(filter)
      .subscribe((result) => (this.rowData = result))
      .add(() => (this.isLoading = false));
  }

  updateColDef(): void {
    this.colDefs = [
      { headerName: 'Username', field: 'userName' },
      { headerName: 'Email', field: 'email' },
      {
        headerName: 'Balance',
        field: 'balance',
        valueFormatter: (params) =>
          this.countryNumber.transform(params.value, this.selectedLocale.value),
      },
      {
        headerName: 'Birth Date',
        field: 'birthDate',
        valueFormatter: (params) =>
          this.dateFormatter.transform(
            new Date(params.value),
            this.selectedLocale.value
          ),
      },
      {
        headerName: 'Phone',
        field: 'phoneNumber',
        cellRenderer: this.phoneCellRenderer.bind(this),
      },
      { headerName: '', cellRenderer: this.videoCallCellRenderer },
    ];
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

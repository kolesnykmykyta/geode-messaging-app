import { Component, OnInit } from '@angular/core';
import { ColDef } from 'ag-grid-community';
import { UsersService } from '../../shared/services/users.service';
import { UserInfo } from '../../shared/interfaces/user-info.interface';
import { Filter } from '../../shared/interfaces/filter.interface';

@Component({
  selector: 'gd-users',
  templateUrl: './users.component.html',
  styleUrl: './users.component.css',
})
export class UsersComponent implements OnInit {
  properties: string[] = ['UserName', 'Email', 'PhoneNumber'];
  rowData: UserInfo[] = [];
  colDefs: ColDef[] = [
    { headerName: 'Username', field: 'userName' },
    { headerName: 'Email', field: 'email' },
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

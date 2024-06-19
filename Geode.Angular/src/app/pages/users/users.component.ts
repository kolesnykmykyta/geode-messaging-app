import { Component, OnInit } from '@angular/core';
import { ColDef } from 'ag-grid-community';
import { UsersService } from './users.service';
import { UserInfo } from './user-info.model';
import { Filter } from '../../shared/models/filter.model';

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
    { headerName: 'Call', cellRenderer: this.videoCallCellRenderer },
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
      .subscribe({
        next: (result) => {
          this.rowData = result;
        },
        error: (error) => {
          console.error('Error occured: ', error.error);
        },
      })
      .add(() => (this.isLoading = false));
  }

  private videoCallCellRenderer(params: any): string {
    const link = `<a href="">Call ${params.data.userName}</a>`;
    return link;
  }

  private phoneCellRenderer(params: any): string {
    const phoneNumber = params.value;
    return `<a style="color: blue; text-decoration: underline; cursor: pointer;" 
            href="tel:${phoneNumber}">${phoneNumber}</a>`;
  }
}

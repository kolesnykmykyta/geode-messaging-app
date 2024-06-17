import { Component, OnInit } from '@angular/core';
import { ColDef } from 'ag-grid-community';
import { UsersService } from './users.service';
import { IUserInfo } from './user-info.dto';
import { FormBuilder, FormGroup } from '@angular/forms';
import { IFilter } from '../../shared/models/filter.model';

@Component({
  selector: 'gd-users',
  templateUrl: './users.component.html',
  styleUrl: './users.component.css',
})
export class UsersComponent implements OnInit {
  properties: string[] = ['UserName', 'Email', 'PhoneNumber'];
  rowData: IUserInfo[] = [];
  colDefs: ColDef[] = [
    { field: 'userName' },
    { field: 'email' },
    { field: 'phoneNumber' },
  ];
  isLoading: boolean = false;

  constructor(
    private usersService: UsersService,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    this.updateRowData();
  }

  updateRowData(filter: IFilter | null = null): void {
    this.isLoading = true;
    this.usersService.getAllUsers(filter).subscribe({
      next: (result) => {
        this.rowData = result;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error occured: ', error.error);
      },
    });
  }
}

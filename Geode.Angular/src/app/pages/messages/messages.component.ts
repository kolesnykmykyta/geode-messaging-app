import { Component, OnInit } from '@angular/core';
import { ColDef } from 'ag-grid-community';
import { Message } from './message.model';
import { MessagesService } from './messages.service';
import { Filter } from '../../shared/models/filter.model';

@Component({
  selector: 'gd-messages',
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.css',
})
export class MessagesComponent implements OnInit {
  properties: string[] = ['Content', 'SentAt'];
  rowData: Message[] = [];
  colDefs: ColDef[] = [{ field: 'content' }, { field: 'sentAt' }];
  isLoading: boolean = false;

  constructor(private messagesService: MessagesService) {}

  ngOnInit(): void {
    this.updateRowData();
  }

  updateRowData(filter: Filter | null = null) {
    this.isLoading = true;
    this.messagesService
      .getAllMessages(filter)
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
}

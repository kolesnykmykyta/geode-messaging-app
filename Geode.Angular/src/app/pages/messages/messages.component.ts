import { Component, OnInit } from '@angular/core';
import { ColDef } from 'ag-grid-community';
import { Message } from '../../shared/interfaces/message.interface';
import { MessagesService } from '../../shared/services/messages.service';
import { Filter } from '../../shared/interfaces/filter.interface';
import {
  MESSAGES_FILTER_PERMISSION,
  MESSAGES_READ_PERMISSION,
} from '../../shared/constants/permissions.constants';

@Component({
  selector: 'gd-messages',
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.css',
})
export class MessagesComponent implements OnInit {
  readonly MESSAGES_READ_PERMISSION = MESSAGES_READ_PERMISSION;
  readonly MESSAGES_FILTER_PERMISSION = MESSAGES_FILTER_PERMISSION;

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
      .subscribe((result) => (this.rowData = result))
      .add(() => (this.isLoading = false));
  }
}

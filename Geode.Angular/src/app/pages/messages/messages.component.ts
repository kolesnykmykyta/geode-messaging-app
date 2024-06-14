import { Component, OnInit } from '@angular/core';
import { ColDef } from 'ag-grid-community';
import { IMessage } from './message.dto';
import { MessagesService } from './messages.service';
import { IFilter } from '../../shared/models/filter.model';

@Component({
  selector: 'gd-messages',
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.css'
})
export class MessagesComponent implements OnInit {
  properties: string[] = ['Content', 'SentAt']
  rowData: IMessage[] = []
  colDefs: ColDef[] = [
    { field: 'content' },
    { field: 'sentAt' },
  ]
  isLoading: boolean = false

  constructor(private messagesService: MessagesService) { }

  ngOnInit(): void {
    this.updateRowData(null)
  }

  updateRowData(filter: IFilter | null){
    this.isLoading = true
    this.messagesService.getAllMessages(filter).subscribe({
      next: (result) => {
        this.rowData = result
        this.isLoading = false
      }
    })
  }
}

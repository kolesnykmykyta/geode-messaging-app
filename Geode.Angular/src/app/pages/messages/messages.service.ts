import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Filter } from '../../shared/models/filter.model';
import { Observable } from 'rxjs';
import { Message } from './message.model';
import { environment } from '../../../environments/environment';
import { partialize } from '../../shared/constants/partialize.constant';

@Injectable({
  providedIn: 'root',
})
export class MessagesService {
  private messagesEndpoint: string = `${environment.apiBase}/messages`;

  constructor(private http: HttpClient) {}

  getAllMessages(filter: Filter | null = null): Observable<Message[]> {
    let requestUrl = `${this.messagesEndpoint}/all`;
    let queryParams = filter ? partialize<Filter>(filter) : {};

    return this.http.get<Message[]>(requestUrl, {
      params: queryParams,
    });
  }
}

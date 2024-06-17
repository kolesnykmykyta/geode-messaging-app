import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Filter } from '../../shared/models/filter.model';
import { Observable } from 'rxjs';
import { Message } from './message.model';
import { environment } from '../../../environments/environment';
import { partialize } from '../../shared/constants/partialize.constant';
import {
  AUTH_RULE_HEADER_NAME,
  AUTH_RULE_HEADER_VALUES,
} from '../../shared/constants/auth-rule-header.constants';

@Injectable({
  providedIn: 'root',
})
export class MessagesService {
  private messagesEndpoint: string = `${environment.apiBase}/messages`;

  constructor(private http: HttpClient) {}

  getAllMessages(filter: Filter | null = null): Observable<Message[]> {
    let requestUrl = `${this.messagesEndpoint}/all`;
    let queryParams = filter ? partialize<Filter>(filter) : {};
    let headers = new HttpHeaders({
      [AUTH_RULE_HEADER_NAME]: AUTH_RULE_HEADER_VALUES.APPLY,
    });

    return this.http.get<Message[]>(requestUrl, {
      params: queryParams,
      headers: headers,
    });
  }
}

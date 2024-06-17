import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IFilter } from '../../shared/models/filter.model';
import { Observable } from 'rxjs';
import { IMessage } from './message.dto';
import { environment } from '../../../environments/environment';
import { partialize } from '../../shared/constants/partialize.constant';

@Injectable({
  providedIn: 'root',
})
export class MessagesService {
  constructor(private http: HttpClient) {}

  getAllMessages(filter: IFilter | null = null): Observable<IMessage[]> {
    let requestUrl = `${environment.apiBase}/messages/all`;
    let queryParams = filter ? partialize<IFilter>(filter) : {};

    return this.http.get<IMessage[]>(requestUrl, {
      params: queryParams,
    });
  }
}

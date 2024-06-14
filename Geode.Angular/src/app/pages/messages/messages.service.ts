import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RequestsHelperService } from '../../shared/services/requests-helper.service';
import { IFilter } from '../../shared/models/filter.model';
import { Observable } from 'rxjs';
import { IMessage } from './message.dto';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {
  constructor(private http: HttpClient, private requestsHelper: RequestsHelperService) { }

  getAllMessages(filter: IFilter | null = null): Observable<IMessage[]>{
    let requestUrl = `${environment.apiBase}/messages/all`
    if (filter != null){
      requestUrl += '?' + this.requestsHelper.generateQueryParamsByFilter(filter)
    }

    return this.http.get<IMessage[]>(requestUrl)
  }
}

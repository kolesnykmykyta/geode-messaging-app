import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IUserInfo } from './user-info.dto';
import { Observable } from 'rxjs';
import { IFilter } from '../../shared/models/filter.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  constructor(private http: HttpClient) { }

  getAllUsers(filter: IFilter | null = null): Observable<IUserInfo[]>{
    let requestUrl = `${environment.apiBase}/user/all`
    if (filter != null){
      const queryString = (Object.keys(filter) as Array<keyof IFilter>)
      .filter(key => !!filter[key])
      .map(key => `${encodeURIComponent(key)}=${encodeURIComponent(filter[key] as string)}`)
      .join('&');

      requestUrl += '?' + queryString
    }

    return this.http.get<IUserInfo[]>(requestUrl);
  }
}

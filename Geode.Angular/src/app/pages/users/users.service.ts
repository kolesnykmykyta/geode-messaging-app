import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IUserInfo } from './user-info.dto';
import { Observable } from 'rxjs';
import { IFilter } from '../../shared/models/filter.model';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  constructor(private http: HttpClient) { }

  getAllUsers(filter: IFilter | null = null): Observable<IUserInfo[]>{
    let requestUrl = "/api/user/all"
    if (filter != null){
      const queryString = (Object.keys(filter) as Array<keyof IFilter>)
      .filter(key => !!filter[key])
      .map(key => `${encodeURIComponent(key as string)}=${encodeURIComponent(filter[key] as any)}`)
      .join('&');

      requestUrl += '?' + queryString
    }

    return this.http.get<IUserInfo[]>(requestUrl);
  }
}

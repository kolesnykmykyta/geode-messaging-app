import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserInfo } from './user-info.model';
import { Observable } from 'rxjs';
import { Filter } from '../../shared/models/filter.model';
import { environment } from '../../../environments/environment';
import { partialize } from '../../shared/constants/partialize.constant';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private usersEndpoint: string = `${environment.apiBase}/user`;

  constructor(private http: HttpClient) {}

  getAllUsers(filter: Filter | null = null): Observable<UserInfo[]> {
    let requestUrl = `${this.usersEndpoint}/all`;
    let queryParams = filter ? partialize<Filter>(filter) : {};

    return this.http.get<UserInfo[]>(requestUrl, {
      params: queryParams,
    });
  }
}

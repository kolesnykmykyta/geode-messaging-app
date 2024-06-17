import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserInfo } from './user-info.model';
import { Observable } from 'rxjs';
import { Filter } from '../../shared/models/filter.model';
import { environment } from '../../../environments/environment';
import { partialize } from '../../shared/constants/partialize.constant';
import {
  AUTH_RULE_HEADER_NAME,
  AUTH_RULE_HEADER_VALUES,
} from '../../shared/constants/auth-rule-header.constants';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private usersEndpoint: string = `${environment.apiBase}/user`;

  constructor(private http: HttpClient) {}

  getAllUsers(filter: Filter | null = null): Observable<UserInfo[]> {
    let requestUrl = `${this.usersEndpoint}/all`;
    let queryParams = filter ? partialize<Filter>(filter) : {};
    let headers = new HttpHeaders({
      [AUTH_RULE_HEADER_NAME]: AUTH_RULE_HEADER_VALUES.APPLY,
    });

    return this.http.get<UserInfo[]>(requestUrl, {
      params: queryParams,
      headers: headers,
    });
  }
}

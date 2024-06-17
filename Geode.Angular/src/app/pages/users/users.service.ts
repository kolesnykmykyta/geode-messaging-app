import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IUserInfo } from './user-info.dto';
import { Observable } from 'rxjs';
import { IFilter } from '../../shared/models/filter.model';
import { environment } from '../../../environments/environment';
import { partialize } from '../../shared/constants/partialize.constant';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  constructor(private http: HttpClient) {}

  getAllUsers(filter: IFilter | null = null): Observable<IUserInfo[]> {
    let requestUrl = `${environment.apiBase}/user/all`;
    let queryParams = filter ? partialize<IFilter>(filter) : {};

    return this.http.get<IUserInfo[]>(requestUrl, {
      params: queryParams,
    });
  }
}

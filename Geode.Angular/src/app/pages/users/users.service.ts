import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IUserInfo } from './user-info.dto';
import { Observable } from 'rxjs';
import { IFilter } from '../../shared/models/filter.model';
import { environment } from '../../../environments/environment';
import { RequestsHelperService } from '../../shared/services/requests-helper.service';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  constructor(
    private http: HttpClient,
    private requestsHelper: RequestsHelperService
  ) {}

  getAllUsers(filter: IFilter | null = null): Observable<IUserInfo[]> {
    let requestUrl = `${environment.apiBase}/user/all`;
    if (filter != null) {
      requestUrl +=
        '?' + this.requestsHelper.generateQueryParamsByFilter(filter);
    }

    return this.http.get<IUserInfo[]>(requestUrl);
  }
}

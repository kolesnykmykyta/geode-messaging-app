import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserInfo } from '../interfaces/user-info.interface';
import { Observable, map } from 'rxjs';
import { Filter } from '../interfaces/filter.interface';
import { environment } from '../../../environments/environment';
import { partialize } from '../constants/partialize.constant';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private usersEndpoint: string = `${environment.apiBase}/user`;

  constructor(private http: HttpClient) {}

  getAllUsers(filter: Filter | null = null): Observable<UserInfo[]> {
    let requestUrl = `${this.usersEndpoint}/all`;
    let queryParams = filter ? partialize<Filter>(filter) : {};

    return this.http
      .get<UserInfo[]>(requestUrl, {
        params: queryParams,
      })
      .pipe(
        map((users) => users.map((user) => this.populateUserWithTestData(user)))
      );
  }

  private populateUserWithTestData(userInfo: UserInfo): UserInfo {
    return {
      ...userInfo,
      balance: userInfo.balance ?? this.generateRandomBalance(),
      birthDate: userInfo.birthDate ?? this.generateRandomDate(),
    };
  }

  private generateRandomBalance(): number {
    return Number((Math.random() * 100000).toFixed(2));
  }

  private generateRandomDate(): Date {
    let startTimestamp = new Date(1970, 1, 1).getTime();
    let endTimestamp = new Date(2005, 31, 12).getTime();
    let randomTimestamp =
      startTimestamp + Math.random() * (endTimestamp - startTimestamp);

    return new Date(randomTimestamp);
  }
}

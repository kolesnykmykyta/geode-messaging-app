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

  private populateUserWithTestData(userInfo: UserInfo) {
    return this.populateUserWithBalance(
      this.populateUserWithBirthDate(userInfo)
    );
  }

  private populateUserWithBalance(
    userInfo: UserInfo,
    balance: number | null = null
  ) {
    balance = balance ?? Number((Math.random() * 100000).toFixed(2)); // Either take provided or generate a random one
    return {
      ...userInfo,
      balance: userInfo.balance ?? balance,
    };
  }

  private populateUserWithBirthDate(
    userInfo: UserInfo,
    birthDate: Date | null = null
  ) {
    let startDate = new Date(1970, 1, 1).getTime();
    let endDate = new Date(2005, 31, 12).getTime();
    let randomDate = startDate + Math.random() * (endDate - startDate);

    return {
      ...userInfo,
      birthDate: userInfo.birthDate ?? new Date(randomDate),
    };
  }
}

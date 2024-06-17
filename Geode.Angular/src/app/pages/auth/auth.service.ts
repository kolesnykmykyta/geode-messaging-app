import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { IRegisterResultDto } from './models/register-result.dto';
import { IRegisterDto } from './models/register.dto';
import { ILoginDto } from './models/login.dto';
import { ITokenDto } from './models/token.dto';

import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import {
  ACCESS_TOKEN_KEY,
  IS_AUTHORIZED_INFO_KEY,
  REFRESH_TOKEN_KEY,
} from '../../shared/constants/storages.constants';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  isUserAuthorizedSignal = signal<boolean>(
    JSON.parse(sessionStorage.getItem(IS_AUTHORIZED_INFO_KEY) ?? 'false')
  );

  constructor(private http: HttpClient) {}

  register(dto: IRegisterDto): Observable<IRegisterResultDto> {
    return this.http.post<IRegisterResultDto>(
      `${environment.apiBase}/user/register`,
      dto
    );
  }

  login(dto: ILoginDto): Observable<ITokenDto> {
    return this.http
      .post<ITokenDto>(`${environment.apiBase}/user/login`, dto)
      .pipe(
        tap((response) => {
          if (response != null) {
            localStorage.setItem(ACCESS_TOKEN_KEY, response.accessToken);
            localStorage.setItem(REFRESH_TOKEN_KEY, response.refreshToken);
            this.updateAuthState(true);
          }
        })
      );
  }

  logout(): void {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    this.updateAuthState(false);
  }

  private updateAuthState(newState: boolean): void {
    this.isUserAuthorizedSignal.set(newState);
    sessionStorage.setItem(IS_AUTHORIZED_INFO_KEY, newState.toString());
  }
}

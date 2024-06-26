import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { RegisterResult } from '../interfaces/auth/register-result.interface';
import { RegisterCredentials } from '../interfaces/auth/register.interface';
import { LoginCredentials } from '../interfaces/auth/login.interface';
import { TokenPair } from '../interfaces/auth/token-pair.interface';

import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import {
  ACCESS_TOKEN_KEY,
  IS_AUTHORIZED_INFO_KEY,
  REFRESH_TOKEN_KEY,
} from '../constants/storages.constants';
import { environment } from '../../../environments/environment';
import {
  AUTH_RULE_HEADER_NAME,
  AUTH_RULE_HEADER_VALUES,
} from '../constants/auth-rule-header.constants';
import {
  MESSAGES_FILTER_PERMISSION,
  MESSAGES_READ_PERMISSION,
  USERS_FILTER_PERMISSION,
  USERS_READ_PERMISSION,
} from '../constants/permissions.constants';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  isUserAuthorized$ = signal<boolean>(
    JSON.parse(sessionStorage.getItem(IS_AUTHORIZED_INFO_KEY) ?? 'false')
  );

  permissions: string[] = [];

  private authEndpoint: string = `${environment.apiBase}/user`;
  private skipAuthHeaders: HttpHeaders = new HttpHeaders({
    [AUTH_RULE_HEADER_NAME]: AUTH_RULE_HEADER_VALUES.SKIP,
  });

  constructor(private http: HttpClient) {}

  register(dto: RegisterCredentials): Observable<RegisterResult> {
    return this.http.post<RegisterResult>(
      `${this.authEndpoint}/register`,
      dto,
      { headers: this.skipAuthHeaders }
    );
  }

  login(dto: LoginCredentials): Observable<TokenPair> {
    return this.http
      .post<TokenPair>(`${this.authEndpoint}/login`, dto, {
        headers: this.skipAuthHeaders,
      })
      .pipe(
        tap((response) => {
          if (response != null) {
            localStorage.setItem(ACCESS_TOKEN_KEY, response.accessToken);
            localStorage.setItem(REFRESH_TOKEN_KEY, response.refreshToken);
            this.updateAuthState(true);
          }
        }),
        tap(() => {
          this.permissions = [
            MESSAGES_READ_PERMISSION,
            MESSAGES_FILTER_PERMISSION,
            USERS_READ_PERMISSION,
          ];
        })
      );
  }

  logout(): void {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    this.permissions = [];
    this.updateAuthState(false);
  }

  private updateAuthState(newState: boolean): void {
    this.isUserAuthorized$.set(newState);
    sessionStorage.setItem(IS_AUTHORIZED_INFO_KEY, newState.toString());
  }
}

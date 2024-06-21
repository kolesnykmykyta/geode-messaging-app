import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { RegisterResult } from './models/register-result.model';
import { RegisterCredentials } from './models/register.model';
import { LoginCredentials } from './models/login.model';
import { TokenPair } from './models/token-pair.model';

import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import {
  ACCESS_TOKEN_KEY,
  IS_AUTHORIZED_INFO_KEY,
  REFRESH_TOKEN_KEY,
} from '../../shared/constants/storages.constants';
import { environment } from '../../../environments/environment';
import { Router } from '@angular/router';
import {
  AUTH_RULE_HEADER_NAME,
  AUTH_RULE_HEADER_VALUES,
} from '../../shared/constants/auth-rule-header.constants';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  isUserAuthorized$ = signal<boolean>(
    JSON.parse(sessionStorage.getItem(IS_AUTHORIZED_INFO_KEY) ?? 'false')
  );

  private authEndpoint: string = `${environment.apiBase}/user`;
  private skipAuthHeaders: HttpHeaders = new HttpHeaders({
    [AUTH_RULE_HEADER_NAME]: AUTH_RULE_HEADER_VALUES.SKIP,
  });

  constructor(private http: HttpClient, private router: Router) {}

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
        })
      );
  }

  logout(): void {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    this.updateAuthState(false);
    this.router.navigateByUrl('/login');
  }

  private updateAuthState(newState: boolean): void {
    this.isUserAuthorized$.set(newState);
    sessionStorage.setItem(IS_AUTHORIZED_INFO_KEY, newState.toString());
  }
}

import { HttpClient } from '@angular/common/http';
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

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  isUserAuthorized$ = signal<boolean>(
    JSON.parse(sessionStorage.getItem(IS_AUTHORIZED_INFO_KEY) ?? 'false')
  );

  private authEndpoint: string = `${environment.apiBase}/user`;

  constructor(private http: HttpClient) {}

  register(dto: RegisterCredentials): Observable<RegisterResult> {
    return this.http.post<RegisterResult>(`${this.authEndpoint}/register`, dto);
  }

  login(dto: LoginCredentials): Observable<TokenPair> {
    return this.http.post<TokenPair>(`${this.authEndpoint}/login`, dto).pipe(
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
    this.isUserAuthorized$.set(newState);
    sessionStorage.setItem(IS_AUTHORIZED_INFO_KEY, newState.toString());
  }
}

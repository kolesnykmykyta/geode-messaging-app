import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { IRegisterResultDto } from './models/register-result.dto';
import { IRegisterDto } from './models/register.dto';
import { ILoginDto } from './models/login.dto';
import { ITokenDto } from './models/token.dto';

import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { ACCESS_TOKEN_NAME, IS_AUTHORIZED_INFO_NAME, REFRESH_TOKEN_NAME } from '../shared/constants/storages.constants';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  isUserAuthorizedSignal = signal<boolean>(JSON.parse(sessionStorage.getItem(IS_AUTHORIZED_INFO_NAME) ?? 'false'))

  constructor(private http: HttpClient) {}

  register(dto: IRegisterDto): Observable<IRegisterResultDto>{
    return this.http.post<IRegisterResultDto>("https://geode-web-app.azurewebsites.net/api/user/register", dto)
  }

  login(dto: ILoginDto): Observable<ITokenDto>{
    return this.http.post<ITokenDto>("https://geode-web-app.azurewebsites.net/api/user/login", dto)
      .pipe(
        tap(response => {
          if (response != null){
            localStorage.setItem(ACCESS_TOKEN_NAME, response.accessToken);
            localStorage.setItem(REFRESH_TOKEN_NAME, response.refreshToken);
            this.updateAuthState(true)
          }
        })
      )
  }

  logout(): void{
    localStorage.removeItem(ACCESS_TOKEN_NAME);
    localStorage.removeItem(REFRESH_TOKEN_NAME);
    this.updateAuthState(false)
  }

  private updateAuthState(newState: boolean): void{
    this.isUserAuthorizedSignal.set(newState)
    sessionStorage.setItem(IS_AUTHORIZED_INFO_NAME, newState.toString())
  }
}

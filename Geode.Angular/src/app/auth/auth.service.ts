import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { IRegisterResultDto } from './models/register-result.dto';
import { IRegisterDto } from './models/register.dto';
import { ILoginDto } from './models/login.dto';
import { ITokenDto } from './models/token.dto';

import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  isUserAuthorizedSignal = signal<boolean>(JSON.parse(sessionStorage.getItem("isAuthorized") ?? 'false'))

  constructor(private http: HttpClient) {}

  register(dto: IRegisterDto): Observable<IRegisterResultDto>{
    return this.http.post<IRegisterResultDto>("https://geode-web-app.azurewebsites.net/api/user/register", dto)
  }

  login(dto: ILoginDto): Observable<ITokenDto>{
    return this.http.post<ITokenDto>("https://geode-web-app.azurewebsites.net/api/user/login", dto)
      .pipe(
        tap(response => {
          if (response != null){
            localStorage.setItem("accessToken", response.accessToken);
            localStorage.setItem("refreshToken", response.refreshToken);
            this.updateAuthState(true)
          }
        })
      )
  }

  logout(): void{
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    this.updateAuthState(false)
  }

  private updateAuthState(newState: boolean): void{
    this.isUserAuthorizedSignal.set(newState)
    sessionStorage.setItem("isAuthorized", newState.toString())
  }
}

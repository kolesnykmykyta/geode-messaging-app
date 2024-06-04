import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { IRegisterResultDto } from './models/register-result.dto';
import { IRegisterDto } from './models/register.dto';
import { ILoginDto } from './models/login.dto';
import { ITokenDto } from './models/token.dto';

import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  isUserAuthorizedSignal = signal<boolean>(false)

  constructor(private http: HttpClient) {
  }

  updateUserAuthorizationInfo(){
    const sessionAuthInfo = sessionStorage.getItem("isAuthorized")
    if (sessionAuthInfo != null){
      this.isUserAuthorizedSignal.set(JSON.parse(sessionAuthInfo))
    }
    else{
      this.isUserAuthorizedSignal.set(false)
    }
  }

  register(dto: IRegisterDto){
    return this.http.post<IRegisterResultDto>("https://geode-web-app.azurewebsites.net/api/user/register", dto)
  }

  login(dto: ILoginDto){
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

  logout(){
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    this.updateAuthState(false)
  }

  private updateAuthState(newState: boolean){
    this.isUserAuthorizedSignal.set(newState)
    sessionStorage.setItem("isAuthorized", newState.toString())
  }
}

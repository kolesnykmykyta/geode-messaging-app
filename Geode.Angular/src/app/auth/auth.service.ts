import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IRegisterResultDto } from './models/register-result.dto';
import { IRegisterDto } from './models/register.dto';
import { ILoginDto } from './models/login.dto';
import { ITokenDto } from './models/token.dto';

import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) {
  }

  register(dto: IRegisterDto){
    return this.http.post<IRegisterResultDto>("https://geode-web-app.azurewebsites.net/api/user/register", dto)
  }

  login(dto: ILoginDto){
    return this.http.post<ITokenDto>("https://geode-web-app.azurewebsites.net/api/user/login", dto)
      .pipe(
        tap(response => {
          if (response != null){
            console.log(response);
            localStorage.setItem("accessToken", response.accessToken);
            localStorage.setItem("refreshToken", response.refreshToken);
          }
        })
      )
  }

  logout(){
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
  }
}

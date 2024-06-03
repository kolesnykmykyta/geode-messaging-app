import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IRegisterResultDto } from './models/register-result.dto';
import { IRegisterDto } from './models/register.dto';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) {
  }

  register(dto: IRegisterDto){
    return this.http.post<IRegisterResultDto>("https://geode-api.azurewebsites.net/api/user/register", dto)
  }
}

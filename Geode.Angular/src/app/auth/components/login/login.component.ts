import { Component } from '@angular/core';

import { ILoginDto } from '../../models/login.dto';
import { AuthService } from '../../auth.service';

@Component({
  selector: 'gd-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  credentials: ILoginDto = {email:'', password: ''}
  message: string = ''

  private authService: AuthService;

  constructor(authService: AuthService) {
    this.authService = authService
  }

  loginSubmit(){
    this.message = "Logging in..."
    this.authService.login(this.credentials)
    .subscribe({
      next: () => {
        this.message = "Successfully logged in!"
      },
      error: () => {
        this.message = "Login has failed. Check your credentials"
      }
    })
  }
}

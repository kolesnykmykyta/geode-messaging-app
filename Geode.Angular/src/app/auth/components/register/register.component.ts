import { Component } from '@angular/core';

import { IRegisterDto } from '../../models/register.dto';
import { IRegisterResultDto } from '../../models/register-result.dto';
import { AuthService } from '../../auth.service';

@Component({
  selector: 'gd-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  credentials: IRegisterDto = {email: '', password: '', username: ''};
  registerResult: IRegisterResultDto = {isSuccess: undefined, errors: []};

  private authService: AuthService;

  constructor(authService: AuthService) {
    this.authService = authService
  }

  registerSubmit(){
    this.registerResult.isSuccess = undefined
    this.authService.register(this.credentials).subscribe({
      next: () => {
        console.log("Successfully registered");
        this.registerResult.isSuccess = true;
      },
      error: (err) => {
        if (err?.error != null){
          this.registerResult = err.error
        }
        else{
          this.registerResult.isSuccess = false
          this.registerResult.errors = ['Unknown error occured']
        }
      }
    })
  }
}

import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { IRegisterDto } from '../../models/register.dto';
import { IRegisterResultDto } from '../../models/register-result.dto';
import { AuthService } from '../../auth.service';

@Component({
  selector: 'gd-register',
  standalone: true,
  imports: [FormsModule, CommonModule],
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
      error: (err) => this.registerResult = err.error
    })
  }
}

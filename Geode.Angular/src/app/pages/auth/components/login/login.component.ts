import { Component } from '@angular/core';

import { ILoginDto } from '../../models/login.dto';
import { AuthService } from '../../auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'gd-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  credentialsForm: FormGroup = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });
  message: string = ''
  private isFormSubmitted: boolean = false

  constructor(private authService: AuthService, private formBuilder: FormBuilder) { }

  loginSubmit(): void{
    this.isFormSubmitted = true
    if (this.credentialsForm.valid){
      this.message = "Logging in..."
      this.authService.login(this.credentialsForm.value as ILoginDto)
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

  getFieldInvalidity(fieldName: string, errorCode: string | null = null): boolean | undefined{
    let formProp = this.credentialsForm.get(fieldName)
    if (errorCode == null){
      return this.isFormSubmitted && formProp?.invalid
    }
    else{
      return this.isFormSubmitted && formProp?.getError(errorCode!)
    }
  }
}

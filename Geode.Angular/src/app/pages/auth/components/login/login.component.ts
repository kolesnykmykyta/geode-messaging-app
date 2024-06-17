import { Component } from '@angular/core';

import { LoginCredentials } from '../../models/login.model';
import { AuthService } from '../../auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'gd-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  credentialsForm: FormGroup = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });
  message: string = '';
  isLoading: boolean = false;

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder
  ) {}

  loginSubmit(): void {
    if (this.credentialsForm.valid) {
      this.message = '';
      this.isLoading = true;
      this.authService
        .login(this.credentialsForm.value as LoginCredentials)
        .subscribe({
          next: () => {
            this.message = 'Successfully logged in!';
          },
          error: () => {
            this.message = 'Login has failed. Check your credentials';
          },
        })
        .add(() => (this.isLoading = false));
    }
  }

  getFieldInvalidity(
    fieldName: string,
    errorCode: string | null = null
  ): boolean | undefined {
    let formProp = this.credentialsForm.get(fieldName);
    if (errorCode == null) {
      return formProp?.touched && formProp?.invalid;
    } else {
      return formProp?.touched && formProp?.getError(errorCode!);
    }
  }
}

import { Component } from '@angular/core';

import { RegisterCredentials } from '../../models/register.model';
import { RegisterResult } from '../../models/register-result.model';
import { AuthService } from '../../auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { passwordValidator } from '../../../../shared/services/password.validator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Component({
  selector: 'gd-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  credentialsForm: FormGroup = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    username: ['', Validators.required],
    password: [
      '',
      [Validators.required, Validators.minLength(6), passwordValidator()],
    ],
  });
  registerResult: RegisterResult = { isSuccess: undefined, errors: [] };
  isLoading: boolean = false;

  private isFormSubmitted: boolean = false;

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    private router: Router
  ) {}

  registerSubmit(): void {
    this.registerResult.isSuccess = undefined;
    this.isFormSubmitted = true;
    this.isLoading = true;
    if (this.credentialsForm.valid) {
      this.authService
        .register(this.credentialsForm.value as RegisterCredentials)
        .subscribe({
          next: () => {
            this.snackBar.open('Successfully registered!', 'Close', {
              duration: 3000,
            });
            this.router.navigateByUrl('/login');
          },
          error: (err) => {
            if (err?.error != null) {
              this.registerResult = err.error;
            } else {
              this.registerResult.isSuccess = false;
              this.registerResult.errors = ['Unknown error occured'];
            }
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

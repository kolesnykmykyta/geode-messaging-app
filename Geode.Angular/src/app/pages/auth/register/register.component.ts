import { Component } from '@angular/core';

import { RegisterCredentials } from '../../../shared/interfaces/auth/register.interface';
import { RegisterResult } from '../../../shared/interfaces/auth/register-result.interface';
import { AuthService } from '../../../shared/services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { passwordValidator } from '../../../shared/constants/password-validator.constant';
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

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    private router: Router
  ) {}

  registerSubmit(): void {
    this.registerResult.isSuccess = undefined;
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
}

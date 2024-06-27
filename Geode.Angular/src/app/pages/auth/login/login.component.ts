import { Component } from '@angular/core';

import { LoginCredentials } from '../../../shared/interfaces/auth/login.interface';
import { AuthService } from '../../../shared/services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

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

  isFailedLogin: boolean = false;
  isLoading: boolean = false;

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    private router: Router
  ) {}

  loginSubmit(): void {
    if (this.credentialsForm.valid) {
      this.isFailedLogin = false;
      this.isLoading = true;
      this.authService
        .login(this.credentialsForm.value as LoginCredentials)
        .subscribe({
          next: () => {
            this.snackBar.open('Successfully logged in!', 'Close', {
              duration: 3000,
            });
            this.router.navigateByUrl('/');
          },
          error: () => {
            this.isFailedLogin = true;
          },
        })
        .add(() => (this.isLoading = false));
    }
  }
}

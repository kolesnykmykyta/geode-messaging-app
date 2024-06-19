import { Component } from '@angular/core';

import { LoginCredentials } from '../../models/login.model';
import { AuthService } from '../../auth.service';
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
  message: string = '';
  isLoading: boolean = false;

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    private router: Router
  ) {}

  loginSubmit(): void {
    if (this.credentialsForm.valid) {
      this.message = '';
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
            this.message = 'Login has failed. Check your credentials';
          },
        })
        .add(() => (this.isLoading = false));
    }
  }
}

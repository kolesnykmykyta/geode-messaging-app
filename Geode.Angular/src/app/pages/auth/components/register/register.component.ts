import { Component } from '@angular/core';

import { IRegisterDto } from '../../models/register.dto';
import { IRegisterResultDto } from '../../models/register-result.dto';
import { AuthService } from '../../auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'gd-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  credentialsForm: FormGroup = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    username: ['', Validators.required],
    password: ['', [Validators.required, Validators.minLength(6)]]
  })
  registerResult: IRegisterResultDto = {isSuccess: undefined, errors: []};
  isLoading: boolean = false;

  private isFormSubmitted: boolean = false;

  constructor(private authService: AuthService, private formBuilder: FormBuilder) {}

  registerSubmit(): void{
    this.registerResult.isSuccess = undefined
    this.isFormSubmitted = true
    this.isLoading = true
    if (this.credentialsForm.valid){
      this.authService.register(this.credentialsForm.value as IRegisterDto).subscribe({
        next: () => {
          console.log("Successfully registered");
          this.registerResult.isSuccess = true;
          this.isLoading = false;
        },
        error: (err) => {
          if (err?.error != null){
            this.registerResult = err.error
          }
          else{
            this.registerResult.isSuccess = false
            this.registerResult.errors = ['Unknown error occured']
          }
          this.isLoading = false;
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
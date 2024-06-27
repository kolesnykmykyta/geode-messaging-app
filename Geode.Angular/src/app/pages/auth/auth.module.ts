import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { AuthRoutingModule } from './auth-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { ErrorHandlerPipe } from '../../shared/pipes/error-handler.pipe';
import { CountryNumberPipe } from '../../shared/pipes/country-number.pipe';
import { DateFormatterPipe } from '../../shared/pipes/date-formatter.pipe';

@NgModule({
  declarations: [RegisterComponent, LoginComponent],
  imports: [CommonModule, AuthRoutingModule, SharedModule, ErrorHandlerPipe],
})
export class AuthModule {}

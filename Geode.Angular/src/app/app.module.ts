import { NgModule } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular'

import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClientModule} from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { RegisterComponent } from './auth/components/register/register.component';
import { NavbarComponent } from './shared/navbar/navbar.component';
import { LoginComponent } from './auth/components/login/login.component';
import { UsersComponent } from './users/users.component';

@NgModule({
  declarations: [
    AppComponent,
    RegisterComponent,
    NavbarComponent,
    LoginComponent,
    UsersComponent,
  ],
  imports: [
    CommonModule,
    RouterOutlet,
    FormsModule,
    HttpClientModule,
    BrowserModule,
    AppRoutingModule,
    AgGridAngular,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

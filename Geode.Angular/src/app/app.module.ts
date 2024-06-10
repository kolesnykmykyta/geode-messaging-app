import { NgModule } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular'

import { RouterOutlet } from '@angular/router';
import { HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { NavbarComponent } from './layout/navbar/navbar.component';
import { UsersComponent } from './users/users.component';
import { AuthModule } from './auth/auth.module';
import { authInterceptor } from './interceptors/auth.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    UsersComponent,
  ],
  imports: [
    RouterOutlet,
    HttpClientModule,
    BrowserModule,
    AppRoutingModule,
    AgGridAngular,
    AuthModule,
  ],
  bootstrap: [AppComponent],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useValue: authInterceptor,
    }
  ]
})
export class AppModule { }

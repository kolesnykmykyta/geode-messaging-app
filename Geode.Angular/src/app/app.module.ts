import { NgModule } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular'

import { RouterOutlet } from '@angular/router';
import { HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { NavbarComponent } from './layout/navbar/navbar.component';
import { UsersComponent } from './pages/users/users.component';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { MatExpansionPanel, MatExpansionPanelHeader, MatExpansionPanelTitle } from '@angular/material/expansion';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthModule } from './pages/auth/auth.module';
import { SharedModule } from './shared/shared.module';
import { MessagesComponent } from './pages/messages/messages.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    UsersComponent,
    MessagesComponent,
  ],
  imports: [
    RouterOutlet,
    HttpClientModule,
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    AgGridAngular,
    AuthModule,
    SharedModule,
    MatExpansionPanel,
    MatExpansionPanelHeader,
    MatExpansionPanelTitle,
  ],
  bootstrap: [AppComponent],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    }
  ]
})
export class AppModule { }

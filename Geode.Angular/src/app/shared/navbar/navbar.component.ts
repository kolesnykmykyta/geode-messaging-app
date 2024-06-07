import { Component } from '@angular/core';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'gd-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  authService: AuthService

  constructor(authService: AuthService) {
    this.authService = authService
  }

  logout(): void{
    this.authService.logout()
  }
}

import { Component } from '@angular/core';
import { AuthService } from '../../pages/auth/auth.service';

@Component({
  selector: 'gd-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  constructor(public authService: AuthService) {}

  logout(): void{
    this.authService.logout()
  }
}

import { Component, OnInit } from '@angular/core';
import { AuthService } from './pages/auth/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Geode';

  constructor(private authService: AuthService){}
}

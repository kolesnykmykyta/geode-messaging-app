import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authorizedGuard: CanActivateFn = (route, state) => {
  let authService = inject(AuthService);
  let router = inject(Router);

  if (authService.isUserAuthorized$()) {
    return true;
  } else {
    router.navigateByUrl('/auth');
    return false;
  }
};

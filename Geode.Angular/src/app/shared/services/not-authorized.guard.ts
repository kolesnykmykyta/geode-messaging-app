import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../pages/auth/auth.service';

export const notAuthorizedGuard: CanActivateFn = (route, state) => {
  let authService = inject(AuthService);
  let router = inject(Router);

  if (authService.isUserAuthorized$()) {
    router.navigateByUrl('/users');
    return false;
  } else {
    return true;
  }
};

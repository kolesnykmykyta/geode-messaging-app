import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { AuthService } from "../../auth/auth.service";

export const authGuard: CanActivateFn = (route, state) => {
    let authService = inject(AuthService);
    let router = inject(Router);

    if (authService.isUserAuthorizedSignal()){
        return true;
    }
    else{
        router.navigateByUrl('/auth');
        return false;
    }
}
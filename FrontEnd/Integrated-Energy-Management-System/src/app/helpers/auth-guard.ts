import { inject } from "@angular/core";
import { AuthenticationService } from "../services/authentication.service";
import { Router } from "@angular/router";


export const AuthGuard = () => {
    const authService = inject(AuthenticationService);
    const router = inject(Router);

    if (authService.isAuthenticated()) {
        return true;
    }

    return router.parseUrl('/login');
}
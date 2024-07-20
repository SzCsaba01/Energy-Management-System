import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router} from '@angular/router';
import { Injectable, inject } from '@angular/core';
import { of, Observable } from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';

export const RoleGuard = (route: ActivatedRouteSnapshot) => {
    const router = inject(Router);
    const authService = inject(AuthenticationService);

    const expectedRoles = route.data['expectedRoles'];
    const role = authService.getUserRole();


    if (!expectedRoles.includes(role)) {
        return router.navigate(['**']);
    }
    return true;
}

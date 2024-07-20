import { Injectable } from "@angular/core";
import { roles } from "../constants/roles";

@Injectable({
    providedIn: 'root'
})
export class RoleChecker {
    isAdmin(userRole: string): boolean {
        return roles.admin == userRole;
    }

    isUser(userRole: string): boolean {
        return roles.user == userRole;
    }
}
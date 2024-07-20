import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AuthenticationService } from "../services/authentication.service";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class AuthenticationInterceptor implements HttpInterceptor {
    constructor(private authenticationService: AuthenticationService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const access_token = this.authenticationService.getAcessToken();
        req = req.clone({
            setHeaders: {
                Authorization: `bearer ${access_token}`
            }
        });
        return next.handle(req);
    }
}
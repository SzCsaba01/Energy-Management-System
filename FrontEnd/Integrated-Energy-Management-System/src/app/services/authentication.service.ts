import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { IAuthentication } from '../models/authentication/authentication.model';
import { map } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private _apiUrl = '';

  constructor(
    private http: HttpClient,
    private route: Router) { 
      this._apiUrl = environment.userApiUrl + 'Authentication';
  }


  public login(userAuthentication: IAuthentication){
    return this.http
      .post<any>(`${this._apiUrl}/Login`, userAuthentication)
      .pipe(
        map((response: {id: string, token: string, role: string, username: string}) => {
          localStorage.setItem('access_token', response.token);
          localStorage.setItem('id', response.id);
          localStorage.setItem('role', response.role);
          localStorage.setItem('username', response.username);
          return response;
        })
      );
  }
  logout(){
    localStorage.removeItem('access_token');
    localStorage.removeItem('id');
    localStorage.removeItem('role');
    this.route.navigate(['/login']);
  }

  getAcessToken(): string | null{
    return localStorage.getItem('access_token')
  }

  isAuthenticated(): boolean{
    let token = this.getAcessToken()
    return (token != null && token != undefined && token != '')
  }

  getUserRole(): string | null{
    return localStorage.getItem('role');
  }

  getUserId(): string | null{
    return localStorage.getItem('id');
  }

  getUsername(): string | null{
    return localStorage.getItem('username');
  }
}
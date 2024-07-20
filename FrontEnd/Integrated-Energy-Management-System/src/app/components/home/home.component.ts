import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  isLoggedIn = false;
  
  constructor (
    private router: Router,
    private authenticationService: AuthenticationService
    ){}


  ngOnInit(): void {
    this.checkIfLoggedIn();
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.checkIfLoggedIn();
      }
    });
  }

  private checkIfLoggedIn(): void {
    this.isLoggedIn = this.authenticationService.isAuthenticated();
  }
}

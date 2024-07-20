import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { takeUntil } from 'rxjs';
import { IAuthentication } from 'src/app/models/authentication/authentication.model';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SelfUnsubscriberBase } from 'src/app/utils/SelfUnsubscriberBase';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent extends SelfUnsubscriberBase implements OnInit {
  isLoginActive!: boolean;

  loginForm: FormGroup; 

  userCredentials: FormControl;
  passwordLogin: FormControl;

  constructor(
    private formBuilder: FormBuilder,
    private authenticationService: AuthenticationService,
    private route: Router,
  ){
    super();
    this.userCredentials = new FormControl('');
    this.passwordLogin = new FormControl('');

    this.loginForm = this.formBuilder.group({
      'userCredentials': this.userCredentials,
      'password': this.passwordLogin,
    })
  }

  ngOnInit(): void {
    this.isLoginActive = true;

    if(this.authenticationService.isAuthenticated()){
      this.route.navigate(['/devices']);
    }
  }

  onSignIn(authentication: IAuthentication){
    this.authenticationService.login(authentication)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(() => {
        this.route.navigate(['/devices']);  
    });
  }
}

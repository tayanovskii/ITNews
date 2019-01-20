import { AuthService } from './../../../Shared/services/auth.service';
import { Component, OnInit } from '@angular/core';
import { HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginData: LoginModel;
  errors: string;

  constructor(private authService: AuthService,
    private router: Router) {
    this.loginData = <LoginModel>{};
  }

  ngOnInit() {
  }

  onSubmit() {
    this.authService.login(this.loginData)
      .subscribe(result => {
        console.log('User was logged in');
        this.router.navigate(['/']);
    }, this.errorHandler);
    console.log(JSON.stringify(this.loginData));
  }

  private errorHandler(error: HttpErrorResponse) {
    this.errors = error.error[''];
    alert(this.errors);
  }
  getErrors() {
    return this.errors;
  }
}

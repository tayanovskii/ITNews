import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from './../../../Shared/services/auth.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {
  registrationData: RegistrationModel;
  errors: '';

  constructor(
    private authService: AuthService,
    private router: Router) {
      this.registrationData = <RegistrationModel>{};
     }

  ngOnInit() {
  }
  onSubmit() {
    console.log(this.registrationData);
    this.authService.registerWithConfirmation(this.registrationData)
      .subscribe(result => {
        console.log(result);
        alert(result);
        this.router.navigate(['/']);
      }, (error: HttpErrorResponse) => console.log('error status -> ' +  error.status));
  }
}

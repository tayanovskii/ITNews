import { AuthService } from './services/auth.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';
import { JwtHelperService } from '@auth0/angular-jwt';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    HttpClientModule,
    JwtModule.forRoot({})
  ],
  providers: [
    AuthService,
    {
      provide: 'BASE_URL',
      useValue: 'https://localhost:5001/',
    },
  ],
  exports: [
    CommonModule
  ]
})
export class SharedModule { }

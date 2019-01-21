import { Injectable, Inject, Injector } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private injector: Injector) {
  }
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const auth = this.injector.get(AuthService);
    const token = (auth. isLoggedIN()) ? auth.getAuth() : null;
    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }
    return next.handle(request);


    // // add authorization header with jwt token if available
    // const tokenLS = JSON.parse(localStorage.getItem(this.token));
    // if (tokenLS) {
    //   request = request.clone({
    //     setHeaders: {
    //       Authorization: `Bearer ${tokenLS}`
    //     }
    //   });
    // }

    // return next.handle(request);
  }
}

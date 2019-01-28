import { AuthService } from 'src/app/Shared/services/auth.service';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(
    private authService: AuthService,
    private router: Router) {}
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
    ): Observable<boolean> | Promise<boolean> | boolean {
      if (this.authService.isLoggedIN()) {
        console.log('user is logged in');
        return true;
      } else {
        console.log('user will be navigate to login page');
        this.router.navigate(['/user/login'], { queryParams: { returnUrl: state.url }});
        return false;
      }
  }
}

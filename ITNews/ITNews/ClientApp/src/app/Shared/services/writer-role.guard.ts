import { AuthService } from 'src/app/Shared/services/auth.service';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WriterRoleGuard implements CanActivate {
  constructor (
    private authService: AuthService,
    private router: Router ) {}
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    console.log(this.authService.getDecodeToken().role);
    console.log('Role in Admin: ' + this.authService.isUserAdmin());
    if (this.authService.isUserAdmin() || this.authService.isUserWriter()) {
      return true;
    } else {
      this.router.navigate(['/no-access']);
      return false;
    }
  }
}

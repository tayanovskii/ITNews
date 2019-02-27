import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/Shared/services/auth.service';
import { Observable, Subscription } from 'rxjs';
import { faSignInAlt, faSignOutAlt } from '@fortawesome/free-solid-svg-icons';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './app-nav-menu.component.html',
  styleUrls: ['./app-nav-menu.component.css']
})
export class AppNavMenuComponent implements OnInit {
  queryString = '';
  username$: Observable<string>;
  isLoggedIn$: Observable<boolean>;
  constructor(
    public authService: AuthService,
    private router: Router) {
     }

  ngOnInit() {
    this.isLoggedIn$ = this.authService.isLoggedIn;
    this.username$ = this.authService.userName;
  }

  logout() {
    this.authService.logout();
  }
  searchNews() {
    console.log(this.queryString);
    this.router.navigate(['/news/search-result', {query: this.queryString}]);
    this.queryString = '';
  }
}

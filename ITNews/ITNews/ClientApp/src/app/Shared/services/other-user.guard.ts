import { NewsService } from './../../News/services/news.service';
import { AuthService } from 'src/app/Shared/services/auth.service';
import { Injectable, NgZone } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, ActivatedRoute } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { News } from 'src/app/News/models/News';

@Injectable()
export class OtherUserGuard implements CanActivate {
  news: News;
  isAccess = false;
  constructor(
    private ngZone: NgZone,
    private authService: AuthService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private newsService: NewsService
  ) { }
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    const newsId = +next.paramMap.get('id');
    console.log('NewsId->', newsId);
      if (newsId) {
        this.ngZone.run(() => {
          this.checkUser(newsId);
        });
        console.log('isAccess: ', this.isAccess);
        return this.isAccess;
       } else {
        return false;
    }
  }
  checkUser(newsId: number) {
    this.ngZone.run(() => {
      this.newsService.getNewsById(newsId)
      .subscribe(res => {
        this.news = res;
        console.log('UserId: ', this.authService.getUserId());
        console.log('UserId from News: ', this.news.userMiniCardDto.userId);
        if (this.authService.getUserId() === this.news.userMiniCardDto.userId
          || this.authService.isUserAdmin()) {
          console.log('Users id is equal');
          this.isAccess = true;
        } else {
          this.isAccess = false;
        }
        console.log('isAccess is -> ', this.isAccess);
      }, error => console.log(error));
    });

  }
}

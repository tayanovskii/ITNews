import { NewsService } from './../../News/services/news.service';
import { AuthService } from 'src/app/Shared/services/auth.service';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { News } from 'src/app/News/models/News';

@Injectable()
export class OtherUserGuard implements CanActivate {
  news: News;
  constructor(
    private authService: AuthService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private newsService: NewsService
    ) {}
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    const newsId = +this.activatedRoute.snapshot.paramMap.get('id');
    console.log('Othe User Guard works');
    if (newsId) {
      this.newsService.getNewsById(newsId)
      .subscribe(res => {
        this.news = res;
        console.log('UserId: ', this.authService.getUserId());
        console.log('UserId from News: ', this.news.userMiniCardDto.userId);
        if (this.authService.getUserId() === this.news.userMiniCardDto.userId
          || this.authService.isUserAdmin()) {
          return true;
        }
        return false;
      });
    } else {
      return false;
    }
  }
}

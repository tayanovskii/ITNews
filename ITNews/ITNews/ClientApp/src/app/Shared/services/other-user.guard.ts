import { NewsService } from './../../News/services/news.service';
import { AuthService } from 'src/app/Shared/services/auth.service';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
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
    if (newsId) {
      this.newsService.getNewsById(newsId)
      .subscribe(res => {
        this.news = res;
        if (this.authService.getUserId() === this.news.userId) {
          return true;
        }
        return false;
      });
    }
    return false;
  }
}
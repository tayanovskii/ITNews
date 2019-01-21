import { HttpClient } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NewsService {

  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl) { }

  createNews(news: SaveNews) {
    const url = this.baseUrl + 'api/news';
    return this.http.post<News>(url, news);
  }
  changeNews(news: SaveNews, id: string) {
    const url = this.baseUrl + 'api/news/' + id;
    return this.http.post<News>(url, news);
  }
  getNewsById() {

  }
  getNews() {}

  getCardNews() {}
  getCardNewsById(id: number) {}
}

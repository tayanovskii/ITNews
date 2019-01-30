import { HttpClient } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';
import { SaveNews } from '../models/SaveNews';

@Injectable({
  providedIn: 'root'
})
export class NewsService {
  url: string;
  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl) {
      this.url = this.baseUrl + 'api/news';
     }

  createNews(news: SaveNews) {
    return this.http.post<News>(this.url, news);
  }
  changeNews(news: SaveNews, id: number) {
    return this.http.post<News>(this.url + id, news);
  }
  getNewsById(newsId: number) {
    return this.http.get<News>(this.url + newsId);
  }
  getForEdit(newsId: number) {
    return this.http.get<SaveNews>(this.url + '/forEdit/' + newsId);
  }
  getNews() {
    return this.http.get<SaveNews[]>(this.url);
  }

  getCardNews() {}
  getCardNewsById(id: number) {}
}

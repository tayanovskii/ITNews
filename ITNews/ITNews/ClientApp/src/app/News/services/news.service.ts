import { HttpClient } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';
import { SaveNews } from '../models/SaveNews';
import { News } from '../models/News';
import { NewsCard } from 'src/app/Shared/models/news-card';

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
  changeNews(news: SaveNews, newsId: number) {
    return this.http.put<News>(this.url + `/${newsId}`, news);
  }
  getNewsById(newsId: number) {
    return this.http.get<News>(this.url + `/${newsId}`);
  }
  getForEdit(newsId: number) {
    return this.http.get<SaveNews>(this.url + '/forEdit/' + newsId);
  }
  getNews() {
    return this.http.get<NewsCard[]>(this.url + '/listCardNews');
  }

  getCardNews() {}
  getCardNewsByUserId(userId: string) {
    return this.http.get<NewsCard[]>(this.url + `/ByUser/` + userId);
  }

}

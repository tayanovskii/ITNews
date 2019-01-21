import { Injectable, Inject } from '@angular/core';
import { Tag } from '../models/tag';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class TagService {

  private url: string;

  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private http: HttpClient) {
      this.url = baseUrl + 'api/Tags';
   }

   getTag(id: number) {
      return this.http.get<Tag>(this.url + id);

   }
   getTags() {
     return this.http.get<Tag[]>(this.url);
   }

   getTagsByPart(tagPart: string) {
      return this.http.get<Tag[]>(this.url + 'byPart');
   }
   getTagByNews(newsId: string) {
    return this.http.get<Tag[]>(this.url + 'byNews');
   }

   createTag(tagName: string) {
     return this.http.post<Tag>(this.url, tagName);
   }
   deleteTag(deletedTagId: number) {
      return this.http.delete(this.url + `/${deletedTagId}`);
   }
}

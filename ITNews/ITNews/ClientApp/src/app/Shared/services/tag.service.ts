import { Injectable, Inject } from '@angular/core';
import { Tag } from '../models/tag';
import { HttpClient } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { TagCloud } from '../models/CloudTag';

@Injectable()
export class TagService {

  private url: string;

  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private http: HttpClient) {
      this.url = baseUrl + 'api/tag';
   }

   getTag(id: number) {
      return this.http.get<Tag>(this.url + `/${id}`);

   }
   getTags() {
     return this.http.get<Tag[]>(this.url).pipe(
       map(res => {
         console.log('tags from service: ' + JSON.stringify(res));
         return res;
       }, catchError(error => error)));
   }
   getTagsForCloud() {
    return this.http.get<TagCloud[]>(this.url + '/tagsCloud').pipe(
      map(res => {
        console.log('tags from service: ' + JSON.stringify(res));
        return res;
      }, catchError(error => error)));
  }
   getTagsByPart(tagPart: string) {
      return this.http.get<Tag[]>(this.url + '/byPart/' + tagPart);
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

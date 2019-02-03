import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class CommentService {

  private url: string;

  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private http: HttpClient) {
      this.url = baseUrl + 'api/comment';
   }

   getComment(id: number) {
      return this.http.get<CommentCard>(this.url + `/${id}`);

   }
   getComments() {
     return this.http.get<CommentCard[]>(this.url).pipe(
       map(res => {
         console.log('comments from service: ' + JSON.stringify(res));
         return res;
       }, catchError(error => error)));
   }

   createComment(comment: SaveComment) {
     return this.http.post<CommentCard>(this.url, comment);
   }
   deleteComment(id: number) {
      return this.http.delete(this.url + `/${id}`);
   }
}

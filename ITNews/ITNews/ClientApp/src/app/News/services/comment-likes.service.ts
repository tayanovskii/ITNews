import { SaveCommentLike } from '../models/SaveCommentLike';
import { CommentLike } from '../models/CommentLike';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CommentLikeService {
  private url: string;
  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private http: HttpClient
  ) {
    this.url = baseUrl + 'api/commentLike';
  }
  setLike(like: SaveCommentLike) {
    return this.http.post<CommentLike>(this.url, like);
  }

  removeLike(deletingLike: SaveCommentLike) {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' }), body: deletingLike };
    return this.http.delete<CommentLike>(this.url, httpOptions);
  }
}

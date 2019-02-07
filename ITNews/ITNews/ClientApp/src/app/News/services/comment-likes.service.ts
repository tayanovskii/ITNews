import { SaveCommentLike } from '../models/SaveCommentLike';
import { CommentLike } from '../models/CommentLike';
import { HttpClient } from '@angular/common/http';
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

  removeLike(likeId: number) {
    return this.http.delete(this.url + `/${likeId}`);
  }
}

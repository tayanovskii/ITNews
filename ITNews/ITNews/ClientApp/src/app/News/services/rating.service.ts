import { SaveNews } from './../models/SaveNews';
import { HttpClient } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';
import { UpdatedRating } from '../models/UpdatedRating';
import { SaveRating } from '../models/SaveRating';

@Injectable()
export class RatingService {
  private url: string;
  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private http: HttpClient
  ) {
    this.url = baseUrl + 'api/rating';
   }
  createRating(newRating: SaveRating) {
    return this.http.post<UpdatedRating>(this.url, newRating);
  }
}

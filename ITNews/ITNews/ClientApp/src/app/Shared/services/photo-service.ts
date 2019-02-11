import { HttpClient } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';

@Injectable()
export class PhotoService {

  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private http: HttpClient
  ) {
    this.baseUrl = 'api/photo';
  }

  createNewsPhoto(file) {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<Photo>(this.baseUrl, formData);
  }

  createAvatar(file, userId: string) {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<Photo>(this.baseUrl + `/${userId}`, formData);
  }
}

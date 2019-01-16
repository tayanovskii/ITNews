import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  url: string;

  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private http: HttpClient) {
      this.url = baseUrl + 'api/Account';
    }

    public CreateUser() {}
}

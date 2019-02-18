import { ManageUser } from './../models/ManageUser';
import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ManageUserService {

  private url: string;
  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private http: HttpClient
  ) {
    this.url = baseUrl + 'api/account';
  }
  getUsers() {
    return this.http.get<ManageUser[]>(this.url + '/manageUsers');
  }
  changeUserProfile(user: ManageUser) {
    return this.http.put(this.url, user);
  }
  deleteUserProfile(id: number) {
    return this.http.delete(this.url + `/${id}`);
  }
}

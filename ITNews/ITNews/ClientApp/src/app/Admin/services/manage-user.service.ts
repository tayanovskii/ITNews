import { ManageUserQueryResult } from './../models/ManageUserQueryResult';
import { ManageUserQuery } from './../models/ManageUserQuery';
import { ManageUser } from './../models/ManageUser';
import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { QueryBuildHelper } from 'src/app/Shared/helpers/QueryBuildHelper';

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
  getUsersByQuery(query: ManageUserQuery) {
    return this.http.get<ManageUserQueryResult>(
      this.url + '/queryManageUsers?' + QueryBuildHelper.getQuery(query));
  }
  changeUserProfile(user: ManageUser) {
    return this.http.put(this.url + `/${user.userId}`, user);
  }
  deleteUserProfile(userId: string) {
    return this.http.delete(this.url + `/${userId}`);
  }
  lockUser(userId: string) {
    return this.http.post(this.url + `/blockUser/${userId}`, {});
  }
  unLockUser(userId: string) {
    return this.http.post(this.url + `/unBlockUser/${userId}`, {});
  }
}

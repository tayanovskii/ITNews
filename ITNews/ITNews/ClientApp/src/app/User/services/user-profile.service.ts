import { HttpClient } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';

@Injectable()
export class UserProfileService {
  private url: string;
  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private http: HttpClient
  ) {
    this.url = baseUrl + 'api/userProfile';
  }
  getUserProfileById(id: number) {
    return this.http.get<UserProfile>(this.url + `/${id}`);
  }
  getUserProfiles() {
    return this.http.get<UserProfile[]>(this.url);
  }
  getUserProfileByUser(userId: string) {
    return this.http.get<UserProfile>(this.url + `/byUser/${userId}`);
  }
  changeUserProfile(userProfile: UserProfile, id: number) {
    return this.http.put(this.url + `/${id}`, userProfile);
  }
  createUserProfile(userProfile: UserProfile) {
    return this.http.post(this.url, userProfile);
  }
  deleteUserProfile(id: number) {
    return this.http.delete(this.url + `/${id}`);
  }
}

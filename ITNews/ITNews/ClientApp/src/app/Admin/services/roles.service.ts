import { Injectable, Inject } from '@angular/core';
import { Role } from '../models/Role';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RolesService {
  private url: string;
  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private http: HttpClient
  ) {
    this.url = baseUrl + 'api/account';
  }
  getRoles() {
    return this.http.get<Role[]>(this.url + '/availableRoles');
  }
}

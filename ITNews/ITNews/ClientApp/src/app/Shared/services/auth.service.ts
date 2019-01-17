import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { Observable, Subject, BehaviorSubject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class AuthService {
  url: string;
  tokenKey: 'ITNewsAuth';
  isLoggedIn: Subject<boolean> = new BehaviorSubject<boolean>(null);
  userName: Subject<string> = new BehaviorSubject<string>(null);

  constructor(
    private http: HttpClient,
    private jwtHelper: JwtHelperService,
    @Inject('BASE_URL') private baseUrl: string
    ) {
      this.url = baseUrl + 'api/account';
    }

    public login(loginData: LoginModel): Observable<boolean> {
      return this.http.post<string>(this.url + '/token', loginData)
        .pipe(
          map(result => {
            if (result != null) {
              this.setAuth(result);
              this.isLoggedIn.next(true);
              return true;
            }
          },
          catchError(error => {
            return new Observable<any>(error);
          })));

    }
    public registerWithoutConfirmation(newUser: SaveUser): Observable<boolean> {
      return this.http.post<string>(this.url + '/register', newUser)
        .pipe(
          map(result => {
            if (result != null) {
              this.setAuth(result);
              return true;
            } else {
              return false;
            }
          }),
          catchError(error => {
            return new Observable<any>(error);
          }));
    }
    public registerWithConfirmation(newUser: SaveUser) { }

    public logout(): boolean {
      this.setAuth(null);
      return true;
    }

    public getDecodeToken(): DecodedToken | null {
      const token = this.getAuth();
      if (token != null) {
        return <DecodedToken> this.jwtHelper.decodeToken(token);
      }
      return null;
    }

    private isExpired(): boolean {
      const token = this.getAuth();
      return this.jwtHelper.isTokenExpired(token);
    }


    private setAuth(token: string | null): boolean {
      if (token) {
        localStorage.setItem(this.tokenKey, token);
        this.isLoggedIn.next(true);
      } else {
        localStorage.removeItem(this.tokenKey);
        this.isLoggedIn.next(false);
      }
      return true;
    }
    private getUserName(): string {
      const token = this.getAuth();
      if (token != null) {
        return this.getDecodeToken().unique_name;
      }
      return null;
    }

    private getAuth(): string | null {
      const token = localStorage.getItem(this.tokenKey);
      return token;
    }
}

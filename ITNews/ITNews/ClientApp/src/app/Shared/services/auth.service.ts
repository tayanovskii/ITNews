import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { Observable, Subject, BehaviorSubject, throwError } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class AuthService {
  private helper = new JwtHelperService();
  private url: string;
  isLoggedIn: Subject<boolean> = new BehaviorSubject<boolean>(this.isLoggedIN());
  userName: Subject<string> = new BehaviorSubject<string>(this.getUserName());
  isAdmin: Subject<boolean> = new BehaviorSubject<boolean>(this.isUserAdmin());

  constructor(
    private http: HttpClient,
    // private jwtHelper: JwtHelperService,
    @Inject('BASE_URL') private baseUrl: string,
    @Inject('TOKEN') private tokenKey: string
    ) {
      this.url = baseUrl + 'api/account';
    }

    public login(loginData: LoginModel): Observable<boolean> {
      return this.http.post<string>(this.url + '/login', loginData)
        .pipe(
          map(result => {
            if (result != null) {
              console.log('token -> ' + result);
              this.setAuth(result);
              this.isLoggedIn.next(true);
              this.userName.next(this.getUserName());
              return true;
            } else {
              console.log('token is null');
            }
          },
          catchError(error => {
            console.log('error:' + JSON.stringify(error));
            return new Observable<any>(error);
          })));

    }
    public registerWithoutConfirmation(newUser: RegistrationModel): Observable<boolean> {
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
            // return new Observable<any>(error);   it works
            return throwError(error);
          }));
    }
    public registerWithConfirmation(newUser: RegistrationModel) {
      return this.http.post<string>(this.url + '/register', newUser)
        .pipe(
          map(result => {
            if (result != null) {
              console.log('answer after registration-> ' + result);
              return result;
            }}),
          catchError(error => {
            return throwError(error);
          }));
    }

    public logout(): boolean {
      this.setAuth(null);
      return true;
    }
    public isLoggedIN(): boolean {
      return this.getAuth() ? true : false;
    }
    public isUserAdmin(): boolean {
      const decodedToken = this.getDecodeToken();
      if (decodedToken) {
        decodedToken.role.forEach(r => {
          if (r.toLowerCase().includes('admin')) {
            return true;
          }
        });
        // return decodedToken.role.includes('admin');
      }
      return false;
    }
    public getAuth(): string | null {
      const token = localStorage.getItem(this.tokenKey);
      return token;
    }
    public getDecodeToken(): DecodedToken | null {
      const token = this.getAuth();
      if (token) {
        return <DecodedToken> this.helper.decodeToken(token);
      }
      return null;
    }

    private isExpired(): boolean {
      const token = this.getAuth();
      return this.helper.isTokenExpired(token);
    }

    private setAuth(token: string | null): boolean {
      if (token) {
        console.log('tokenKey-> ' + this.tokenKey);
        localStorage.setItem(this.tokenKey, token);
        this.isLoggedIn.next(true);
      } else {
        localStorage.removeItem(this.tokenKey);
        this.isLoggedIn.next(false);
      }
      return true;
    }
    public getUserName(): string | null {
      const decodedToken = this.getDecodeToken();
      if (decodedToken) {
        return decodedToken.unique_name;
      }
      return null;
    }
    public getUserId(): string {
      const decodedToken = this.getDecodeToken();
      if (decodedToken) {
        return decodedToken.sub;
      }
      return null;
    }
}

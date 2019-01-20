import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { Observable, Subject, BehaviorSubject, throwError } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class AuthService {
  private url: string;
  private tokenKey = 'ITNews_access_token';
  isLoggedIn: Subject<boolean> = new BehaviorSubject<boolean>(null);
  userName: Subject<string> = new BehaviorSubject<string>(null);
  private helper = new JwtHelperService();


  constructor(
    private http: HttpClient,
    // private jwtHelper: JwtHelperService,
    @Inject('BASE_URL') private baseUrl: string
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

    public getDecodeToken(): DecodedToken | null {
      const token = this.getAuth();
      if (token != null) {
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

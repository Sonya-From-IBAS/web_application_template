import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Register } from '../shared/models/register';
import { Login } from '../shared/models/login';
import { User } from '../shared/models/user';
import { environment } from '../../environments/environment.development';
import { map, of, ReplaySubject } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private userInfo = new ReplaySubject<User | null>(1);
  user$ = this.userInfo.asObservable();

  constructor(private http: HttpClient, private router: Router) {}

  register(model: Register) {
    return this.http.post(`${environment.appUrl}/api/account/register`, model);
  }

  login(model: Login) {
    return this.http.post<User>(`${environment.appUrl}/api/account/login`, model).pipe(
      //костыль для получения user из data ответа(
      map((res: any) => {
        console.log(res)
        if(res) {
          this.setUser(res.data);
        }
      })
    );
  }

  private setUser(user: User) {
    localStorage.setItem(environment.userKey, JSON.stringify(user));
    this.userInfo.next(user);
  }

  getUserJWT() {
    const userData = localStorage.getItem(environment.userKey);
    if(userData !== null) {
      const user: User = JSON.parse(userData);
      return user.jwt;
    }
    return null;
  }

  refreshJWT(jwt: string | null) {
    if (jwt === null) {
      this.userInfo.next(null)
      return of(undefined);
    }
    let headers = new HttpHeaders();
    headers = headers.set("Authorization", "Bearer " + jwt);
    
    return this.http.get<User>(`${environment.appUrl}/api/account/refresh-user-token`, {headers}).pipe(
      map((res: any) => {
        console.log(res);
        if(res) {
          this.setUser(res);
        }
      })
    );
  }

  logout() {
    localStorage.removeItem(environment.userKey);
    this.userInfo.next(null)
    this.router.navigateByUrl("/");
  }
}


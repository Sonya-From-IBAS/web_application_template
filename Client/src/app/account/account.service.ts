import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Register } from '../shared/models/register';
import { Login } from '../shared/models/login';
import { User } from '../shared/models/user';
import { environment } from '../../environments/environment.development';
import { map, ReplaySubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private userInfo = new ReplaySubject<User | null>(1);
  user$ = this.userInfo.asObservable();

  constructor(private http: HttpClient) {}

  register(model: Register) {
    return this.http.post('http://localhost:5063/api/account/register', model);
  }

  login(model: Login) {
    return this.http.post<User>('http://localhost:5063/api/account/login', model).pipe(
      //костыль для получения user из value ответа(
      map((user: any) => {
        console.log(user)
        if(user) {
          this.setUser(user.data);
          return user;
        }
        return null;
      })
    );
  }

  private setUser(user: User) {
    localStorage.setItem(environment.userKey, JSON.stringify(user));
    this.userInfo.next(user);
  }
}


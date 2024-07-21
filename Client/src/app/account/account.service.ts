import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Register } from '../shared/models/register';
import { Login } from '../shared/models/login';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http: HttpClient) {}

  register(model: Register) {
    return this.http.post('http://localhost:5063/api/account/register', model);
  }

  login(model: Login) {
    return this.http.post('http://localhost:5063/api/account/login', model);
  }
}


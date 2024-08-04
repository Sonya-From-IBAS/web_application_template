import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, take } from 'rxjs';
import { AccountService } from '../../account/account.service';
import { User } from '../models/user';

@Injectable()
export class jwtInterceptor implements HttpInterceptor{

  constructor(private accountService: AccountService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    console.log('intercept сработал!');
    this.accountService.user$.pipe(take(1)).subscribe({
      next: (user: User | null) => {
        if(user) {
          console.log('next')
          req = req.clone({
            setHeaders: {
              Authorization: `Bearer ${user.jwt}`
            }
          })
        }
      }
    });
    return next.handle(req);
  }
}
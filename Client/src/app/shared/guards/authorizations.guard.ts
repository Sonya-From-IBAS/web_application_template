import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { AccountService } from '../../account/account.service';
import { SharedService } from '../shared.service';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationGuard {
  constructor(
    private accountService: AccountService,
    private sharedService: SharedService,
    private router: Router
  ) {}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    return this.accountService.user$.pipe(
      map((user: User | null) => {
        if(user != null) {
          return true;
        } else {
          this.sharedService.showNotification(false, "Restricted Area", "You must authorize to stay here");
          this.router.navigate(["account/login"], {queryParams: {returnUrl: state.url}});
          return false;
        }
      })
    )
  }
}
import { Component, OnInit } from '@angular/core';
import { AccountService } from './account/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {

  constructor(private accountService: AccountService) {}

  ngOnInit(): void {
    console.log("init");
    this.refreshJWT();
  }

  private refreshJWT() {
    const jwt = this.accountService.getUserJWT();
    console.log(jwt);
    if(jwt) {
      console.log('jwt refresh')
      this.accountService.refreshJWT(jwt).subscribe({
        next: _ => {},
        error: _ => {
          this.accountService.logout();
        }
      })
    } else {
      this.accountService.refreshJWT(null).subscribe();
    }
  }
}

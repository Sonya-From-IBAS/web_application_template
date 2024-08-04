import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { AccountService } from '../account.service';
import { SharedService } from '../../shared/shared.service';
import { take } from 'rxjs';
import { User } from '../../shared/models/user';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit{
  loginForm: FormGroup = new FormGroup({});
  submitted: boolean = false;
  errorMessages: string[] = [];
  returnUrl: string | null = null;

  constructor(
    private accountService: AccountService, 
    private formBuilder: FormBuilder, 
    private router: Router,
    private sharedService: SharedService,
    private activatedRoute: ActivatedRoute){
      //если уже зарегистрировавшись перейти по /account/login
      this.accountService.user$.pipe(
        take(1)
      ).subscribe({
        next: (user: User | null) => {
          if(user) {
            this.router.navigateByUrl("/");
          } else {
            this.activatedRoute.queryParams.subscribe({
              next: (params: Params) => {
                console.log(params)
                if(params) {
                  this.returnUrl = params['returnUrl'];
                }
                console.log(this.returnUrl);
              }
            })
          }
        }
      })
    }

    ngOnInit(): void {
      this.initForm();
    }

  initForm(): void {
    this.loginForm = this.formBuilder.group({
      userName: ['', Validators.required],
      password: ['', Validators.required],
    })
  }

  login() {
    this.submitted = true;
    this.errorMessages = [];

    if(this.loginForm.valid) {
      this.accountService.login(this.loginForm.value).subscribe({
        next: (res: any) => {
          console.log(res);
          if(this.returnUrl) {
            this.router.navigateByUrl(this.returnUrl);
          } else {
            this.router.navigateByUrl("/");
          }
          // this.sharedService.showNotification(true, 'authorization', res.value);
          // this.router.navigateByUrl('/account/login');
        },
        error: (res: any) => {
          console.log(res);
          if(res.error){
            this.errorMessages = res.error;
          }
        }
      });
    }
  }

}

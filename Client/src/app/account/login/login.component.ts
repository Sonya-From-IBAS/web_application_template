import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../account.service';
import { SharedService } from '../../shared/shared.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit{
  loginForm: FormGroup = new FormGroup({});
  submitted: boolean = false;
  errorMessages: string[] = []; 

  constructor(
    private accountService: AccountService, 
    private formBuilder: FormBuilder, 
    private router: Router,
    private sharedService: SharedService){}

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
          this.sharedService.showNotification(true, 'authorization', res.value);
          this.router.navigateByUrl('/account/login');
        },
        error: (res: any) => {
          if(res.error.errors){
            this.errorMessages = res.error.errors;
          } else {
            this.errorMessages.push(res.error);
          }
        }
      });
    }
  }

}

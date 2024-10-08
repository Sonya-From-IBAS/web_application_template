import { Component, OnInit } from '@angular/core';
import { AccountService } from '../account.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SharedService } from '../../shared/shared.service';
import { Router } from '@angular/router';
import { take } from 'rxjs';
import { User } from '../../shared/models/user';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit{
  registerForm: FormGroup = new FormGroup({});
  submitted = false;
  errorMessages: string[] = [];

  constructor(
    private accountService: AccountService, 
    private formBuilder: FormBuilder, 
    private sharedService: SharedService,
    private router: Router){
      //если уже зарегистрировавшись перейти по /account/register
      this.accountService.user$.pipe(
        take(1)
      ).subscribe({
        next: (user: User | null) => {
          if(user){
            this.router.navigateByUrl("/");
          }
        }
      })
    }
  
  ngOnInit(): void {
    this.initForm();
  }

  initForm(): void {
    this.registerForm = this.formBuilder.group({
      firstName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(20)]],
      lastName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(20)]],
      email: ['', [Validators.required, Validators.pattern('^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$')]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]],
    })
  }

  register():void {
    this.submitted = true;
    this.errorMessages = [];

    if(this.registerForm.valid) {
      this.accountService.register(this.registerForm.value).subscribe({
        next: (res: any) => {
          console.log(res);
          this.sharedService.showNotification(true, 'Registration', res.data);
          this.router.navigateByUrl('/account/login');
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

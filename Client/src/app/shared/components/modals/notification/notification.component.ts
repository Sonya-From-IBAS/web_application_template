import { Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrl: './notification.component.css'
})
export class NotificationComponent {
  isSucceed: boolean = true;
  title: string = '';
  text: string = ''

  constructor(public modalRef: BsModalRef){}
}

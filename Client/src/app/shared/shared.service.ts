import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { NotificationComponent } from './components/modals/notification/notification.component';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  private modalRef?: BsModalRef;
  constructor(private modalService: BsModalService) { }

  showNotification(isSucceed: boolean, title: string, text: string){
    const initialValues: ModalOptions = {
      initialState: {
        isSucceed,
        title,
        text
      }
    };
    this.modalRef = this.modalService.show(NotificationComponent, initialValues);
  }
}

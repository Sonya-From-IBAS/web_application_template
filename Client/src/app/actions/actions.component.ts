import { Component, OnInit } from '@angular/core';
import { ActionsService } from './actions.service';

@Component({
  selector: 'app-actions',
  templateUrl: './actions.component.html',
  styleUrl: './actions.component.css'
})
export class ActionsComponent implements OnInit{

  message: string | undefined;

  constructor(private actionsService: ActionsService) {}

  ngOnInit(): void {
    this.actionsService.getActions().subscribe({
      next: (res: any) =>{
        console.log(res);
        this.message = res.data;
      },
      error: err => console.log(err)
    })
  }
}

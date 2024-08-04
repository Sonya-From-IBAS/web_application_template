import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ActionsService {

  constructor(private http: HttpClient) { }

  getActions(): Observable<Object> {
    return this.http.get(`${environment.appUrl}/api/actions/get-actions`);
  }
}

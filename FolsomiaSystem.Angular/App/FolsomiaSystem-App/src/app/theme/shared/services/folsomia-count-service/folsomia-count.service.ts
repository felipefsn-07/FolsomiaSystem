import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';  
import { HttpHeaders } from '@angular/common/http';  
import { Observable } from 'rxjs'; 
import {FolsomiaCount } from '../../models/folsomia-count';
import { environment } from '../../../../../environments/environment'
 

var httpOptions = {headers: new HttpHeaders({"Content-Type": "application/json"})};


@Injectable({
  providedIn: 'root'
})
export class FolsomiaCountService {

  
  url = environment.folsomiaApi +environment.folsomiaCount;
  constructor(private http: HttpClient) { } 
  folsomiaCount(folsomiaCount: FolsomiaCount): Observable<FolsomiaCount> {  
    return this.http.post<FolsomiaCount>(this.url, folsomiaCount, httpOptions);  
  }
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';  
import { HttpHeaders } from '@angular/common/http';  
import { Observable } from 'rxjs'; 
import {FolsomiaSetup } from '../../models/folsomia-setup';
import { environment } from '../../../../../environments/environment'
 

var httpOptions = {headers: new HttpHeaders({"Content-Type": "application/json"})};


@Injectable({
  providedIn: 'root'
})
export class FolsomiaSetupService {

  private headers: HttpHeaders;
  private accessPointUrl: string = environment.folsomiaApi +environment.folsomiaSetup;

  constructor(private http: HttpClient ) { 
    this.headers = new HttpHeaders({'Content-Type': 'application/json; charset=utf-8'});

  } 

  public getSetup() { 
    
    return this.http.get(this.accessPointUrl, {headers: this.headers});

  }  

}

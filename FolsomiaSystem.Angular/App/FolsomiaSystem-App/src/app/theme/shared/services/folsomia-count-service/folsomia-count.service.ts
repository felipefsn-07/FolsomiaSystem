import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest } from '@angular/common/http';  
import { HttpHeaders } from '@angular/common/http';  
import { Observable, throwError } from 'rxjs'; 
import {FolsomiaCount, FolsomiaCountInput } from '../../models/folsomia-count';
import { environment } from '../../../../../environments/environment'
import { catchError } from 'rxjs/operators';
import 'rxjs/add/operator/map';


var httpOptions = {headers: new HttpHeaders({"Content-Type": "application/json"})};


@Injectable({
  providedIn: 'root'
})
export class FolsomiaCountService {

  
  private headers: HttpHeaders;
  private accessPointUrl: string = environment.folsomiaApi+environment.folsomiaCount;
  private folsomiaResult:FolsomiaCount;

  constructor(private http: HttpClient ) { 
    this.headers = new HttpHeaders({'Content-Type': 'application/json'});

  } 


  public countFolsomia(folsomiaCount:FolsomiaCountInput) {

      return this.http.post<FolsomiaCount>(this.accessPointUrl, JSON.stringify(folsomiaCount), {headers: this.headers})


}


/*
  public async countFolsomia(folsomiaCount:FolsomiaCountInput) {
    


    const t = await this.http.post<FolsomiaCount>(this.accessPointUrl, JSON.stringify(folsomiaCount), {headers: this.headers}).toPromise();
    this.folsomiaResult = new FolsomiaCount();
    this.folsomiaResult.fileResult.fileAsBase64 = t.fileResult.fileAsBase64;
    this.folsomiaResult.idTest = t.idTest;
    this.folsomiaResult.totalCountFolsomia = t.totalCountFolsomia;
    console.log(this.folsomiaResult.idTest);

    return this.folsomiaResult;
}


  public  countFolsomia(folsomiaCount:FolsomiaCountInput): Observable<FolsomiaCount> {
    return this.http.post<FolsomiaCount>(this.accessPointUrl, folsomiaCount,{headers: this.headers});
  }*/

 /* public async countFolsomia(folsomiaCount:FolsomiaCountInput) {
    try {
     return this.http
        .post(this.accessPointUrl, JSON.stringify(folsomiaCount), {headers: this.headers}).toPromise();

      } catch (error) {
      //await this.handleError(error);
    }
  }*/
  

  public count(folsomiaCount:FolsomiaCountInput) {
    return this.http.post(this.accessPointUrl, folsomiaCount, {headers: this.headers})
    .pipe(
      catchError((err) => {
        console.log('error caught in service')
        console.error(err);

        //Handle the error here

        return throwError(err);    //Rethrow it back to component
      }) )

  }
}



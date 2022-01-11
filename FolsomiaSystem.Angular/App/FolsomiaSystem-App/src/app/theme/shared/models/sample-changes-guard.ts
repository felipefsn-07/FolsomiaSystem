import { CanDeactivate } from "@angular/router";  
import { Injectable } from "@angular/core";  
import { Observable } from "rxjs/Observable";  
  
@Injectable()
export class CanDeactivateGuard {
  canDeactivate(): boolean {
   
      if (confirm("É possível que as alterações feitas não sejam salvas.")) {
          return true;
      } else {
          return false;
      }
  }
}
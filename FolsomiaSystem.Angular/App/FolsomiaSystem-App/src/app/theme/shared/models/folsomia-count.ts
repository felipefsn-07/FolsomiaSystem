import { DecimalPipe } from '@angular/common';
import { AuditLog } from './audit-log';
import { FileResult} from './file-result';
export class FolsomiaCount {
    idTest: string;
    totalCountFolsomia:number;
    fileResult:FileResult;
    fileAsBase64:string;
    auditLog:AuditLog;
    concetration:number;

    constructor(){
        this.idTest = "";
        this.fileAsBase64 = "";
        this.totalCountFolsomia = 0;
        this.fileResult = new FileResult();
        this.auditLog = new AuditLog();
    }
}

export class FolsomiaCountInput{
    fileAsBase64:string;
}
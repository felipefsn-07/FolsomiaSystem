import { DecimalPipe } from '@angular/common';
import { BackgroundImage } from '../enum/BackgroundImage';
import { AuditLog } from './audit-log';
import { FileResult} from './file-result';
export class FolsomiaCount {
    idTest: string;
    totalCountFolsomia:number;
    backgroundImage:BackgroundImage;
    fileResult:FileResult;
    fileAsBase64:string;
    auditLog:AuditLog;
    concetration:number;

    constructor(){
        this.idTest = "";
        this.fileAsBase64 = "";
        this.backgroundImage = BackgroundImage.Default;
        this.totalCountFolsomia = 0;
        this.concetration = 0;
        this.fileResult = new FileResult();
        this.auditLog = new AuditLog();
    }
}

export class FolsomiaCountInput{
    fileAsBase64:string;
    backgroundImage:BackgroundImage;
}
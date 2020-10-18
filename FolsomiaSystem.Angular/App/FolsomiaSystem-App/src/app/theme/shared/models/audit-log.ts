import { OperationLog } from '../enum/operation-log';
import { StatusLog } from '../enum/status-log';

export class AuditLog{
    auditLogId:number;
    messageLog:string;
    dateTime:string;
    operationLog:OperationLog;
    statusLog:StatusLog;




}
CREATE SCHEMA IF NOT EXISTS folsomiasystemtest;
USE folsomiasystemtest;
CREATE TABLE IF NOT EXISTS Tb_Folsomia_Setup_Test (
	FolsomiaSetupId	INTEGER NOT NULL PRIMARY KEY auto_increment,
	MaxTest	int NOT NULL,
	MaxConcentration	int NOT NULL
);
CREATE TABLE IF NOT EXISTS Tb_Audit_Log_Test (
	AuditLogId INTEGER NOT NULL PRIMARY KEY auto_increment,
	MessageLog varchar (1000) NULL,
	DateLog datetime NOT NULL,
	OperationLog	int NOT NULL,
	StatusLog	int NOT NULL
);

INSERT INTO Tb_Folsomia_Setup_Test (FolsomiaSetupId,MaxTest,MaxConcentration) VALUES (1,5,20);
COMMIT;

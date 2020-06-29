CREATE SCHEMA IF NOT EXISTS folsomiasystem;
USE folsomiasystem;
CREATE TABLE IF NOT EXISTS Tb_Folsomia_Setup (
	FolsomiaSetupId	INTEGER NOT NULL PRIMARY KEY auto_increment,
	MaxTest	int NOT NULL,
	MaxConcentration	int NOT NULL
);
CREATE TABLE IF NOT EXISTS Tb_Audit_Log (
	AuditLogId INTEGER NOT NULL PRIMARY KEY auto_increment,
	MessageLog varchar (1000),
	DateLog datetime NOT NULL,
	OperationLog	int NOT NULL,
	StatusLog	int NOT NULL
);
CREATE TABLE IF NOT EXISTS tb_user_admin (
	AdminUserId INTEGER NOT NULL PRIMARY KEY auto_increment,
	UserName varchar (1000),
	Password varchar(1000) NOT NULL
);

INSERT INTO tb_user_admin (AdminUserId, UserName, Password) VALUES (1, 'admin', '21232f297a57a5a743894a0e4a801fc3');
INSERT INTO Tb_Folsomia_Setup (FolsomiaSetupId,MaxTest,MaxConcentration) VALUES (1,5,20);
COMMIT;
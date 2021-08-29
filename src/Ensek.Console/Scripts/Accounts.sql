PRAGMA foreign_keys=OFF;
BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "Accounts" (
	"Id"	INTEGER,
	"FirstName"	TEXT,
	"LastName"	TEXT,
	PRIMARY KEY("Id")
);
INSERT INTO Accounts VALUES(1234,'Freya','Test');
INSERT INTO Accounts VALUES(1239,'Noddy','Test');
INSERT INTO Accounts VALUES(1240,'Archie','Test');
INSERT INTO Accounts VALUES(1241,'Lara','Test');
INSERT INTO Accounts VALUES(1242,'Tim','Test');
INSERT INTO Accounts VALUES(1243,'Graham','Test');
INSERT INTO Accounts VALUES(1244,'Tony','Test');
INSERT INTO Accounts VALUES(1245,'Neville','Test');
INSERT INTO Accounts VALUES(1246,'Jo','Test');
INSERT INTO Accounts VALUES(1247,'Jim','Test');
INSERT INTO Accounts VALUES(1248,'Pam','Test');
INSERT INTO Accounts VALUES(2233,'Barry','Test');
INSERT INTO Accounts VALUES(2344,'Tommy','Test');
INSERT INTO Accounts VALUES(2345,'Jerry','Test');
INSERT INTO Accounts VALUES(2346,'Ollie','Test');
INSERT INTO Accounts VALUES(2347,'Tara','Test');
INSERT INTO Accounts VALUES(2348,'Tammy','Test');
INSERT INTO Accounts VALUES(2349,'Simon','Test');
INSERT INTO Accounts VALUES(2350,'Colin','Test');
INSERT INTO Accounts VALUES(2351,'Gladys','Test');
INSERT INTO Accounts VALUES(2352,'Greg','Test');
INSERT INTO Accounts VALUES(2353,'Tony','Test');
INSERT INTO Accounts VALUES(2355,'Arthur','Test');
INSERT INTO Accounts VALUES(2356,'Craig','Test');
INSERT INTO Accounts VALUES(4534,'JOSH','TEST');
INSERT INTO Accounts VALUES(6776,'Laura','Test');
INSERT INTO Accounts VALUES(8766,'Sally','Test');
INSERT INTO Accounts VALUES(8767,'Joe','Bloggs');
INSERT INTO Accounts VALUES(8768,'Fred','Bloggsy');
COMMIT;

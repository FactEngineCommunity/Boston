-- DBWScript v4.1
-- Database: C:\ProgramData\Viev Pty Ltd\Boston\2.5.0.0\database\boston.vdb

CREATE TABLE [ClientServerFunction] (
	[FunctionName] TEXT(50) WITH COMPRESSION,
	[FunctionFullText] TEXT(255) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([FunctionName])
);
ALTER TABLE [ClientServerFunction] DENY ZERO LENGTH [FunctionName];
ALTER TABLE [ClientServerFunction] DENY ZERO LENGTH [FunctionFullText];

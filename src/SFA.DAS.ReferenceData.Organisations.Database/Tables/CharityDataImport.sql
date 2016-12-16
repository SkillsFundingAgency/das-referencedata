CREATE TABLE [CharityData].[CharityDataImport]
(
	[Id] int NOT NULL PRIMARY KEY IDENTITY,
	[Month] int NOT NULL,
	[Year] int NOT NULL,
	[ImportDate] DATETIME NOT NULL DEFAULT GETDATE()
)

CREATE TABLE charitydata.charitynamesearch
(
	RegNo int CONSTRAINT [PK_RegNo] PRIMARY KEY,
	Name varchar(500)
)
GO

CREATE FULLTEXT CATALOG ftCharityCatalog as DEFAULT
GO
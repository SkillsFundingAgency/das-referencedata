CREATE TABLE charitydata.charitynamesearch
(
	RegNo int CONSTRAINT [PK_RegNo] PRIMARY KEY,
	Name varchar(500)
)
GO

CREATE FULLTEXT CATALOG ftCharityCatalog as DEFAULT
GO

IF NOT EXISTS(SELECT 1 FROM sys.fulltext_indexes where object_id = 430624577)
BEGIN
	CREATE FULLTEXT INDEX ON [CharityData].charitynamesearch ([name]) KEY INDEX [PK_RegNo] ON [ftCharityCatalog] WITH CHANGE_TRACKING AUTO
END
GO

CREATE TABLE charitydata.charitynamesearch
(
	RegNo int CONSTRAINT [PK_RegNo] PRIMARY KEY,
	Name varchar(250)
)
GO

CREATE FULLTEXT INDEX ON [CharityData].charitynamesearch ([name]) KEY INDEX [PK_RegNo] ON [ftCharityCatalog] WITH CHANGE_TRACKING AUTO
GO

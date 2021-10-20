
IF NOT EXISTS(SELECT object_id  FROM sys.fulltext_indexes WHERE EXISTS (SELECT object_id  FROM sys.fulltext_indexes  where object_id = object_id('[CharityData].[charitynamesearch]')))
BEGIN
	CREATE FULLTEXT INDEX ON [CharityData].charitynamesearch ([name]) KEY INDEX [PK_RegNo] ON [ftCharityCatalog] WITH CHANGE_TRACKING AUTO
END
GO


IF NOT EXISTS(SELECT name  FROM sysindexes WHERE EXISTS (SELECT name  FROM sysindexes  WHERE name =  'IDX_Charity_RegNo_SubNo'))
BEGIN
	CREATE INDEX IDX_Charity_RegNo_SubNo ON [CharityData].[charity] ([regno],[subno]) INCLUDE ([add1],[add2],[add3],[add4],[add5],[name],[orgtype],[postcode]) 
END
GO


IF NOT EXISTS(SELECT name  FROM sysindexes WHERE EXISTS (SELECT name  FROM sysindexes  WHERE name =  'IDX_Registration_RegNo_SubNo'))
BEGIN
	CREATE INDEX IDX_Registration_RegNo_SubNo ON [CharityData].[registration] ([regno],[subno]) INCLUDE ([regdate])
END
GO

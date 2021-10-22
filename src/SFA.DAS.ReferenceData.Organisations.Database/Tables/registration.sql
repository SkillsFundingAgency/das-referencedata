CREATE TABLE [CharityData].[registration]
(
	[regno] [int] NULL ,
	[subno] [int] NULL,
	[regdate] [smalldatetime] NULL,
	[remdate] [smalldatetime] NULL,
	[remcode] [char](3) NULL
)
GO

IF NOT EXISTS(SELECT name  FROM sysindexes WHERE EXISTS (SELECT name  FROM sysindexes  WHERE name =  'IDX_Registration_RegNo_SubNo'))
BEGIN
	CREATE INDEX IDX_Registration_RegNo_SubNo ON [CharityData].[registration] ([regno],[subno]) INCLUDE ([regdate]) WITH (ONLINE = ON)
END
GO
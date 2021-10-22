﻿CREATE TABLE [CharityData].[charity]
(
	[regno] [int] NULL,
	[subno] [int] NULL,
	[name] [varchar](500) NULL DEFAULT '',
	[orgtype] [varchar](50) NULL,
	[gd] [nvarchar](max) NULL,
	[aob] [varchar](max) NULL,
	[aob_defined] [int] NULL,
	[nhs] [varchar](1) NULL DEFAULT '',
	[ha_no] [int] NULL,
	[corr] [varchar](255) NULL,
	[add1] [varchar](35) NULL,
	[add2] [varchar](35) NULL,
	[add3] [varchar](35) NULL,
	[add4] [varchar](35) NULL,
	[add5] [varchar](35) NULL,
	[postcode] [varchar](8) NULL,
	[phone] [varchar](400) NULL,
	[fax] [int] NULL
)
GO


IF NOT EXISTS(SELECT name  FROM sysindexes WHERE EXISTS (SELECT name  FROM sysindexes  WHERE name =  'IDX_Charity_RegNo_SubNo'))
BEGIN
	CREATE INDEX IDX_Charity_RegNo_SubNo ON [CharityData].[charity] ([regno],[subno]) INCLUDE ([add1],[add2],[add3],[add4],[add5],[name],[orgtype],[postcode]) WITH (ONLINE = ON)
END
GO
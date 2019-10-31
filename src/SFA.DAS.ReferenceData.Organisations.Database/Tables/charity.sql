﻿CREATE TABLE [CharityData].[charity]
(
	[regno] [int] NULL,
	[subno] [int] NULL,
	[name] [varchar](150) NULL,
	[orgtype] [varchar](10) NULL,
	[gd] [nvarchar](max) NULL,
	[aob] [varchar](max) NULL,
	[aob_defined] [int] NULL,
	[nhs] [varchar](1) NULL,
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

ALTER TABLE [CharityData].[charity]
ADD CONSTRAINT df_charity_0 DEFAULT '' FOR [name];
GO

ALTER TABLE [CharityData].[charity]
ADD CONSTRAINT df_charity_1 DEFAULT '' FOR [nhs];
GO

CREATE INDEX IDX_Charity_RegNo_SubNo ON [CharityData].[charity] ([regno],[subno]) INCLUDE ([add1],[add2],[add3],[add4],[add5],[name],[orgtype],[postcode]) 
GO
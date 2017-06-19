CREATE TABLE [CharityData].[registration]
(
	[regno] [int] NULL ,
	[subno] [int] NULL,
	[regdate] [smalldatetime] NULL,
	[remdate] [smalldatetime] NULL,
	[remcode] [char](3) NULL
)
GO

CREATE INDEX IDX_Registration_RegNo_SubNo ON [CharityData].[registration] ([regno],[subno])

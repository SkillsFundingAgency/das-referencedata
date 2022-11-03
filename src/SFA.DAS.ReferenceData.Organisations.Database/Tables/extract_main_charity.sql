CREATE TABLE [CharityImport].[extract_main_charity]
(
	[regno] [int] NULL,
	[coyno] [varchar] (100) NULL,
	[trustees] [varchar](1) NOT NULL,
	[fyend] [varchar](4) NULL,
	[welsh] [varchar](1) NOT NULL,
	[incomedate] [smalldatetime] NULL,
	[income] [numeric](12, 0) NULL,
	[grouptype] [varchar](3) NULL,
	[email] [varchar](400) NULL,
	[web] [varchar](400) NULL
)

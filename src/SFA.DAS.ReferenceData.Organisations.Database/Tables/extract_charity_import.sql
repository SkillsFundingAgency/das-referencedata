CREATE TABLE [CharityImport].[extract_charity_import]
(	
	[DateOfExtract] [smalldatetime] NULL, --1
	[OrganisationNumber] [int] NULL, -- 2
	[RegisteredCharityNumber] [int] NULL, --3
	[LinkedCharityNumber] [int] NULL, -- 4
	[CharityName] [varchar](500) NULL, --5
	[CharityType] [varchar](50) NULL, --6
	[CharityRegistrationStatus] [varchar](50) NULL, --7 
	[DateOfRegistration] [smalldatetime] NULL, --8
	[DateOfRemoval] [smalldatetime] NULL, --9
	[CharityReportingStatus] [varchar](150) NULL, --10
	[LatestAccFinPeriodStartDate] [smalldatetime] NULL, --11
	[LatestAccFinPeriodEndDate] [smalldatetime] NULL, --12
	LatestIncome [numeric](12, 0) NULL, --13
	[LatestExpenditure] [numeric](12, 0) NULL, --13
	[CharityContactAddress1] [varchar](50) NULL, --14
	[CharityContactAddress2] [varchar](50) NULL, --15
	[CharityContactAddress3] [varchar](50) NULL, --16
	[CharityContactAddress4] [varchar](50) NULL, --17
	[CharityContactAddress5] [varchar](50) NULL, --18
	[CharityContactPostcode] [varchar](8) NULL, --19
	[CharityContactPhone] [varchar](100) NULL, --20
	[CharityContactEmail] [varchar](100) NULL, --21 
	[CharityContactWeb] [varchar](400) NULL, -- 22
	[CharityCompanyRegistrationNumber] [varchar](100) NULL, --23
	[CharityInsolvent] [bit] NULL, -- 24
	[CharityInAdministration] [bit] NULL, --25
	[CharityPreviouslyExcepted] [bit] NULL, --26
	[CharityIsCdfOrCif] [varchar](100) NULL, --27
	[CharityIsCio] [bit] NULL, --28
	[CioIsDissolved] [bit] NULL, --29
	[DateCioDissolutionNotice]  [smalldatetime] NULL, --30
	[CharityActivities] [nvarchar](max) NULL, --31
	[CharityGiftAid] [bit] NULL, -- 32
	[CharityHasLand] [bit] NULL, -- 33
)
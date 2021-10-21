CREATE PROCEDURE [CharityData].[GetPublicExtractCharityToStagingTables]
WITH EXECUTE AS OWNER
AS

INSERT INTO CharityImport.extract_charity
      ([regno]  ,[subno] ,[name] ,[orgtype] ,[gd]  ,[aob]  ,[aob_defined] ,[nhs] ,[ha_no] ,[corr] ,[add1] ,[add2] ,[add3] ,[add4] ,[add5] ,[postcode] ,[phone] ,[fax])
SELECT RegisteredCharityNumber as [regno]  
	, LinkedCharityNumber as [subno] 
	, CharityName as [name]
	, CharityRegistrationStatus as [orgtype]
	, null as [gd]
	, null as [aob]
	, 0 as [aob_defined]
	, 'F' as nhs
	, 0 as [ha_no]
	, null as [corr]
	, CharityContactAddress1 as [add1]
	, CharityContactAddress2 as [add2]
	, CharityContactAddress3 as [add3]
	, CharityContactAddress4 as [add4]
	, CharityContactAddress5 as [add5]
	, CharityContactPostcode as [postcode]
	, CharityContactPhone as [phone]
	, 0 as [fax]
FROM [CharityImport].[extract_charity_import]

	
INSERT INTO [CharityImport].[extract_registration]
      ([regno] ,[subno] ,[regdate]  ,[remdate] ,[remcode])
SELECT RegisteredCharityNumber as [regno]  
      , LinkedCharityNumber as [subno] 
      , DateOfRegistration as [regdate]
      , DateOfRemoval as [remdate]
      , 0 as  [remcode]
FROM [CharityImport].[extract_charity_import]


INSERT INTO [CharityImport].[extract_main_charity]
      ([regno] ,[coyno] ,[trustees] ,[fyend] ,[welsh] ,[incomedate] ,[income] ,[grouptype] ,[email] ,[web])
SELECT RegisteredCharityNumber as [regno]
      ,CharityCompanyRegistrationNumber as [coyno]
      , 'F' as [trustees]
      , FORMAT(LatestAccFinPeriodEndDate, 'ddMM') as [fyend]
      , 'F' as [welsh]
      , LatestAccFinPeriodEndDate as [incomedate]
      , LatestIncome as [income]
      , null as [grouptype]
      , CharityContactEmail as [email]
      , CharityContactWeb as [web]
 FROM [CharityImport].[extract_charity_import]

GO



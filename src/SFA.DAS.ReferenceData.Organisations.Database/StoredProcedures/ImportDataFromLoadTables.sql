CREATE PROCEDURE [CharityData].[ImportDataFromLoadTables]
	WITH EXECUTE AS OWNER
AS
	IF OBJECT_ID('[CharityData].acct_submit', 'U') IS NOT NULL TRUNCATE TABLE [CharityData].acct_submit
	IF OBJECT_ID('[CharityData].aoo_ref', 'U') IS NOT NULL TRUNCATE TABLE [CharityData].aoo_ref
	IF OBJECT_ID('[CharityData].ar_submit', 'U') IS NOT NULL TRUNCATE TABLE [CharityData].ar_submit
	IF OBJECT_ID('[CharityData].charity', 'U') IS NOT NULL TRUNCATE TABLE [CharityData].charity
	IF OBJECT_ID('[CharityData].charity_aoo', 'U') IS NOT NULL TRUNCATE TABLE [CharityData].charity_aoo
	IF OBJECT_ID('[CharityData].class', 'U') IS NOT NULL TRUNCATE TABLE [CharityData].class
	IF OBJECT_ID('[CharityData].class_ref', 'U') IS NOT NULL TRUNCATE TABLE [CharityData].class_ref
	IF OBJECT_ID('[CharityData].financial', 'U') IS NOT NULL TRUNCATE TABLE [CharityData].financial
	IF OBJECT_ID('[CharityData].main_charity', 'U') IS NOT NULL TRUNCATE TABLE [CharityData].main_charity
	IF OBJECT_ID('[CharityData].name', 'U') IS NOT NULL TRUNCATE TABLE [CharityData].name
	IF OBJECT_ID('[CharityData].[objects]', 'U') IS NOT NULL TRUNCATE TABLE [CharityData].[objects]
	IF OBJECT_ID('[CharityData].overseas_expend', 'U') IS NOT NULL TRUNCATE TABLE [CharityData].overseas_expend
	IF OBJECT_ID('[CharityData].partb', 'U') IS NOT NULL TRUNCATE TABLE [CharityData].partb
	IF OBJECT_ID('[CharityData].registration', 'U') IS NOT NULL TRUNCATE TABLE [CharityData].registration
	IF OBJECT_ID('[CharityData].remove_ref', 'U') IS NOT NULL TRUNCATE TABLE [CharityData].remove_ref
	IF OBJECT_ID('[CharityData].trustee', 'U') IS NOT NULL TRUNCATE TABLE [CharityData].trustee
	IF OBJECT_ID('[CharityData].charitynamesearch', 'U') IS NOT NULL TRUNCATE TABLE [CharityData].charitynamesearch

	insert into [CharityData].acct_submit select * from [CharityImport].extract_acct_submit
	insert into [CharityData].aoo_ref select * from [CharityImport].extract_aoo_ref
	insert into [CharityData].ar_submit select * from [CharityImport].extract_ar_submit
	insert into [CharityData].charity select * from [CharityImport].extract_charity
	insert into [CharityData].charity_aoo select * from [CharityImport].extract_charity_aoo
	insert into [CharityData].class select * from [CharityImport].extract_class
	insert into [CharityData].class_ref select * from [CharityImport].extract_class_ref
	insert into [CharityData].financial select * from [CharityImport].extract_financial
	insert into [CharityData].main_charity select * from [CharityImport].extract_main_charity
	insert into [CharityData].name    select * from [CharityImport].extract_name
	insert into [CharityData].[objects] select * from [CharityImport].extract_objects
	insert into [CharityData].overseas_expend select * from [CharityImport].extract_overseas_expend
	insert into [CharityData].partb select * from [CharityImport].extract_partb
	insert into [CharityData].registration select * from [CharityImport].extract_registration
	insert into [CharityData].remove_ref select * from [CharityImport].extract_remove_ref
	insert into [CharityData].trustee  select * from [CharityImport].extract_trustee

	insert into  charitydata.charitynamesearch (regno, name)
	select c.regno,rtrim(name) + '-' + cast(c.regno as varchar) as name from charitydata.charity  c
	inner join charitydata.registration r on r.regno = c.regno and r.subno = c.subno
	inner join charitydata.main_charity mc on mc.regno = c.regno
	where r.subno = 0 and orgtype <> 'RM' and remcode is null
	group by name, c.regno

CREATE PROCEDURE [CharityData].[ImportDataFromLoadTables]
	WITH EXECUTE AS OWNER
AS

	IF OBJECT_ID('[CharityData].acct_submit', 'U') IS NOT NULL DROP TABLE [CharityData].acct_submit
	IF OBJECT_ID('[CharityData].aoo_ref', 'U') IS NOT NULL DROP TABLE [CharityData].aoo_ref
	IF OBJECT_ID('[CharityData].ar_submit', 'U') IS NOT NULL DROP TABLE [CharityData].ar_submit
	IF OBJECT_ID('[CharityData].charity', 'U') IS NOT NULL DROP TABLE [CharityData].charity
	IF OBJECT_ID('[CharityData].charity_aoo', 'U') IS NOT NULL DROP TABLE [CharityData].charity_aoo
	IF OBJECT_ID('[CharityData].class', 'U') IS NOT NULL DROP TABLE [CharityData].class
	IF OBJECT_ID('[CharityData].class_ref', 'U') IS NOT NULL DROP TABLE [CharityData].class_ref
	IF OBJECT_ID('[CharityData].financial', 'U') IS NOT NULL DROP TABLE [CharityData].financial
	IF OBJECT_ID('[CharityData].main_charity', 'U') IS NOT NULL DROP TABLE [CharityData].main_charity
	IF OBJECT_ID('[CharityData].name', 'U') IS NOT NULL DROP TABLE [CharityData].name
	IF OBJECT_ID('[CharityData].[objects]', 'U') IS NOT NULL DROP TABLE [CharityData].[objects]
	IF OBJECT_ID('[CharityData].overseas_expend', 'U') IS NOT NULL DROP TABLE [CharityData].overseas_expend
	IF OBJECT_ID('[CharityData].partb', 'U') IS NOT NULL DROP TABLE [CharityData].partb
	IF OBJECT_ID('[CharityData].registration', 'U') IS NOT NULL DROP TABLE [CharityData].registration
	IF OBJECT_ID('[CharityData].remove_ref', 'U') IS NOT NULL DROP TABLE [CharityData].remove_ref
	IF OBJECT_ID('[CharityData].trustee', 'U') IS NOT NULL DROP TABLE [CharityData].trustee
	IF OBJECT_ID('[CharityData].charitynamesearch', 'U') IS NOT NULL DELETE FROM charitydata.charitynamesearch

	select * into [CharityData].acct_submit from [CharityImport].extract_acct_submit
	select * into [CharityData].aoo_ref from [CharityImport].extract_aoo_ref
	select * into [CharityData].ar_submit from [CharityImport].extract_ar_submit
	select * into [CharityData].charity from [CharityImport].extract_charity
	select * into [CharityData].charity_aoo from [CharityImport].extract_charity_aoo
	select * into [CharityData].class from [CharityImport].extract_class
	select * into [CharityData].class_ref from [CharityImport].extract_class_ref
	select * into [CharityData].financial from [CharityImport].extract_financial
	select * into [CharityData].main_charity from [CharityImport].extract_main_charity
	select * into [CharityData].name    from [CharityImport].extract_name
	select * into [CharityData].[objects] from [CharityImport].extract_objects
	select * into [CharityData].overseas_expend from [CharityImport].extract_overseas_expend
	select * into [CharityData].partb from [CharityImport].extract_partb
	select * into [CharityData].registration from [CharityImport].extract_registration
	select * into [CharityData].remove_ref from [CharityImport].extract_remove_ref
	select * into [CharityData].trustee  from [CharityImport].extract_trustee



	insert into  charitydata.charitynamesearch (regno, name)
	select c.regno,rtrim(name) + '-' + cast(c.regno as varchar) as name from charitydata.charity  c
	inner join charitydata.registration r on r.regno = c.regno and r.subno = c.subno
	inner join charitydata.main_charity mc on mc.regno = c.regno
	where r.subno = 0 and orgtype <> 'RM' and remcode is null
	group by name, c.regno

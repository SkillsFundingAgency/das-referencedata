CREATE PROCEDURE [CharityData].[ImportDataFromLoadTables]
	WITH EXECUTE AS OWNER
AS
	IF OBJECT_ID('[CharityData].charity', 'U') IS NOT NULL TRUNCATE TABLE [CharityData].charity
	IF OBJECT_ID('[CharityData].main_charity', 'U') IS NOT NULL TRUNCATE TABLE [CharityData].main_charity
	IF OBJECT_ID('[CharityData].registration', 'U') IS NOT NULL TRUNCATE TABLE [CharityData].registration
	IF OBJECT_ID('[CharityData].charitynamesearch', 'U') IS NOT NULL TRUNCATE TABLE [CharityData].charitynamesearch

	insert into [CharityData].charity select * from [CharityImport].extract_charity
	insert into [CharityData].main_charity select * from [CharityImport].extract_main_charity
	insert into [CharityData].registration select * from [CharityImport].extract_registration

	insert into  charitydata.charitynamesearch (regno, name)
	select c.regno,rtrim(name) + '-' + cast(c.regno as varchar) as name from charitydata.charity  c
	inner join charitydata.registration r on r.regno = c.regno and r.subno = c.subno
	inner join charitydata.main_charity mc on mc.regno = c.regno
	where r.subno = 0 and orgtype <> 'RM' and remcode is null
	group by name, c.regno

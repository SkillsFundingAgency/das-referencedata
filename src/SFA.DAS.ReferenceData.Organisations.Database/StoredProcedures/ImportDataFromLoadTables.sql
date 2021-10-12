CREATE PROCEDURE [CharityData].[ImportDataFromLoadTables]
	WITH EXECUTE AS OWNER
AS
	IF OBJECT_ID('[CharityData].charity', 'U') IS NOT NULL DROP TABLE [CharityData].charity
	IF OBJECT_ID('[CharityData].main_charity', 'U') IS NOT NULL DROP TABLE [CharityData].main_charity
	IF OBJECT_ID('[CharityData].registration', 'U') IS NOT NULL DROP TABLE [CharityData].registration
	IF OBJECT_ID('[CharityData].charitynamesearch', 'U') IS NOT NULL DELETE FROM charitydata.charitynamesearch

	select * into [CharityData].charity from [CharityImport].extract_charity
	select * into [CharityData].main_charity from [CharityImport].extract_main_charity
	select * into [CharityData].registration from [CharityImport].extract_registration

	insert into  charitydata.charitynamesearch (regno, name)
	select c.regno,rtrim(name) + '-' + cast(c.regno as varchar) as name from charitydata.charity  c
	inner join charitydata.registration r on r.regno = c.regno and r.subno = c.subno
	inner join charitydata.main_charity mc on mc.regno = c.regno
	where r.subno = 0 and orgtype <> 'RM' and remcode is null
	group by name, c.regno

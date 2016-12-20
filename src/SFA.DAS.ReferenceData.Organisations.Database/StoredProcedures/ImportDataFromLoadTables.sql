CREATE PROCEDURE [CharityData].[ImportDataFromLoadTables]
AS

	TRUNCATE TABLE [CharityData].acct_submit
	TRUNCATE TABLE [CharityData].aoo_ref
	TRUNCATE TABLE [CharityData].ar_submit
	TRUNCATE TABLE [CharityData].charity
	TRUNCATE TABLE [CharityData].charity_aoo
	TRUNCATE TABLE [CharityData].class
	TRUNCATE TABLE [CharityData].class_ref
	TRUNCATE TABLE [CharityData].financial
	TRUNCATE TABLE [CharityData].main_charity
	TRUNCATE TABLE [CharityData].name
	TRUNCATE TABLE [CharityData].[objects]
	TRUNCATE TABLE [CharityData].overseas_expend
	TRUNCATE TABLE [CharityData].partb
	TRUNCATE TABLE [CharityData].registration
	TRUNCATE TABLE [CharityData].remove_ref
	TRUNCATE TABLE [CharityData].trustee

	insert into [CharityData].acct_submit with(tablock) select * from [CharityImport].extract_acct_submit
	insert into [CharityData].aoo_ref with(tablock) select * from [CharityImport].extract_aoo_ref
	insert into [CharityData].ar_submit with(tablock) select * from [CharityImport].extract_ar_submit
	insert into [CharityData].charity with(tablock) select * from [CharityImport].extract_charity
	insert into [CharityData].charity_aoo  with(tablock) select * from [CharityImport].extract_charity_aoo
	insert into [CharityData].class with(tablock) select * from [CharityImport].extract_class
	insert into [CharityData].class_ref with(tablock) select * from [CharityImport].extract_class_ref
	insert into [CharityData].financial with(tablock) select * from [CharityImport].extract_financial
	insert into [CharityData].main_charity with(tablock) select * from [CharityImport].extract_main_charity
	insert into [CharityData].name with(tablock) select * from [CharityImport].extract_name
	insert into [CharityData].[objects] with(tablock) select * from [CharityImport].extract_objects
	insert into [CharityData].overseas_expend with(tablock) select * from [CharityImport].extract_overseas_expend
	insert into [CharityData].partb with(tablock) select * from [CharityImport].extract_partb
	insert into [CharityData].registration with(tablock) select * from [CharityImport].extract_registration
	insert into [CharityData].remove_ref with(tablock) select * from [CharityImport].extract_remove_ref
	insert into [CharityData].trustee with(tablock) select * from [CharityImport].extract_trustee


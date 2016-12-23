CREATE PROCEDURE [CharityData].[ImportDataFromLoadTables]
AS

	DROP TABLE [CharityData].acct_submit
	DROP TABLE [CharityData].aoo_ref
	DROP TABLE [CharityData].ar_submit
	DROP TABLE [CharityData].charity
	DROP TABLE [CharityData].charity_aoo
	DROP TABLE [CharityData].class
	DROP TABLE [CharityData].class_ref
	DROP TABLE [CharityData].financial
	DROP TABLE [CharityData].main_charity
	DROP TABLE [CharityData].name
	DROP TABLE [CharityData].[objects]
	DROP TABLE [CharityData].overseas_expend
	DROP TABLE [CharityData].partb
	DROP TABLE [CharityData].registration
	DROP TABLE [CharityData].remove_ref
	DROP TABLE [CharityData].trustee

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

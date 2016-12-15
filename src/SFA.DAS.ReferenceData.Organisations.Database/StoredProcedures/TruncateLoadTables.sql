CREATE PROCEDURE [CharityImport].[TruncateLoadTables]
AS

TRUNCATE TABLE [CharityImport].extract_acct_submit
TRUNCATE TABLE [CharityImport].extract_aoo_ref
TRUNCATE TABLE [CharityImport].extract_ar_submit
TRUNCATE TABLE [CharityImport].extract_charity
TRUNCATE TABLE [CharityImport].extract_charity_aoo
TRUNCATE TABLE [CharityImport].extract_class
TRUNCATE TABLE [CharityImport].extract_class_ref
TRUNCATE TABLE [CharityImport].extract_financial
TRUNCATE TABLE [CharityImport].extract_main_charity
TRUNCATE TABLE [CharityImport].extract_name
TRUNCATE TABLE [CharityImport].extract_objects
TRUNCATE TABLE [CharityImport].extract_overseas_expend
TRUNCATE TABLE [CharityImport].extract_partb
TRUNCATE TABLE [CharityImport].extract_registration
TRUNCATE TABLE [CharityImport].extract_remove_ref
TRUNCATE TABLE [CharityImport].extract_trustee



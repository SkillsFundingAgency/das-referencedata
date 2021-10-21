CREATE PROCEDURE [CharityData].[TruncateLoadTables]
    WITH EXECUTE AS OWNER
AS

TRUNCATE TABLE [CharityImport].extract_charity_import
TRUNCATE TABLE [CharityImport].extract_charity
TRUNCATE TABLE [CharityImport].extract_main_charity
TRUNCATE TABLE [CharityImport].extract_registration
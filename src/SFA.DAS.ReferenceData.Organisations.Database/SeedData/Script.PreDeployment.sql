/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
IF NOT EXISTS(SELECT 1 FROM sys.fulltext_catalogs where [name] = 'ftCharityCatalog')
BEGIN
	CREATE FULLTEXT CATALOG ftCharityCatalog as DEFAULT
END

IF EXISTS(select 1  from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'charity' and COLUMN_NAME='name' and IS_NULLABLE = 'YES')
BEGIN
	ALTER TABLE [CharityData].[charity]
	ALTER COLUMN name VARCHAR(150) NOT NULL
END
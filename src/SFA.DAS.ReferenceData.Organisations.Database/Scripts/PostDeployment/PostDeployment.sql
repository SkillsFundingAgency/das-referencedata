-- /*
--  Post-Deployment Script Template							
-- --------------------------------------------------------------------------------------
--  This file contains SQL statements that will be executed before the build script.	
--  Use SQLCMD syntax to include a file in the pre-deployment script.			
--  Example:      :r .\myfile.sql								
--  Use SQLCMD syntax to reference a variable in the pre-deployment script.		
--  Example:      :setvar TableName MyTable							
--                SELECT * FROM [$(TableName)]					
-- --------------------------------------------------------------------------------------
-- */

ALTER TABLE [CharityData].[charity]  ALTER COLUMN name VARCHAR (500) NULL;
ALTER TABLE [CharityData].[charity]  ALTER COLUMN orgtype VARCHAR (50) NULL;

ALTER TABLE [CharityData].[charitynamesearch]  ALTER COLUMN Name VARCHAR (500) NULL;
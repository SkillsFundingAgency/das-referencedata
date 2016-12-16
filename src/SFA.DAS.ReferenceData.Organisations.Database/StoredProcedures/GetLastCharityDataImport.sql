CREATE PROCEDURE [CharityData].[GetLastCharityDataImport]
AS
	select top 1 [Month], [Year], [ImportDate] from [CharityData].[CharityDataImport]
	order by [Year] desc, [Month] desc
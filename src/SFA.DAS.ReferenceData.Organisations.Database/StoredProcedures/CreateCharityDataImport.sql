CREATE PROCEDURE [CharityData].[CreateCharityDataImport]
	@month int,
	@year int
AS
	insert into [CharityData].[CharityDataImport]
	([Month], [Year])
	values
	(@month, @year)
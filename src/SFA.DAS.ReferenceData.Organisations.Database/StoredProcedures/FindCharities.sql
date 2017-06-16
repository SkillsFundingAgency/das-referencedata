CREATE PROCEDURE [CharityData].[FindCharities]
(
	@SearchTerm NVARCHAR(4000),
	@MaximumResults INT
)
AS

	DECLARE @wildcardedSearch NVARCHAR(4000) = '%' + LOWER(@SearchTerm) + '%'

	select top (@MaximumResults)
	charity.regno as 'RegistrationNumber',
	rtrim(charity.name) as 'Name',
	rtrim(charity.add1) as 'Address1',
	rtrim(charity.add2) as 'Address2',
	rtrim(charity.add3) as 'Address3',
	rtrim(charity.add4) as 'Address4',
	rtrim(charity.add5) as 'Address5',
	rtrim(charity.postcode) as 'PostCode',
	registration.regdate as 'RegistrationDate',
	CAST (CASE WHEN charity.orgtype = 'RM' THEN 1 ELSE 0 END AS BIT) 'IsRemoved'
	from
	[CharityData].[charity] charity
	inner join [CharityData].[registration] registration on registration.regno = charity.regno and registration.subno = charity.subno
	where
	LOWER(charity.name) LIKE @wildcardedSearch
	

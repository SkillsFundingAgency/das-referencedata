CREATE PROCEDURE [CharityData].[GetCharityByRegistrationNumber]
(
	@RegistrationNumber int
)
AS

	select
	charity.regno as 'RegistrationNumber',
	rtrim(charity.name) as 'Name',
	rtrim(charity.add1) as 'Address1',
	rtrim(charity.add2) as 'Address2',
	rtrim(charity.add3) as 'Address3',
	rtrim(charity.add4) as 'Address4',
	rtrim(charity.add5) as 'Address5',
	registration.regdate as 'RegistrationDate',
	CAST (CASE WHEN charity.orgtype = 'RM' THEN 1 ELSE 0 END AS BIT) 'IsRemoved'
	from
	[CharityData].[charity] charity
	inner join [CharityData].[registration] registration on registration.regno = charity.regno and registration.subno = charity.subno
	where
	charity.regno = @RegistrationNumber
	and charity.subno = 0
	

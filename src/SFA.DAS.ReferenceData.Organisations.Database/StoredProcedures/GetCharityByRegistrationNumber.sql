CREATE PROCEDURE [CharityData].[GetCharityByRegistrationNumber]
(
	@RegistrationNumber int
)
AS

	select
	charity.regno as 'RegistrationNumber',
	charity.name as 'Name',
	charity.add1 as 'Address1',
	charity.add2 as 'Address2',
	charity.add3 as 'Address3',
	charity.add4 as 'Address4',
	charity.add5 as 'Address5',
	registration.regdate as 'RegistrationDate'
	from
	[CharityData].[charity] charity
	inner join [CharityData].[registration] registration on registration.regno = charity.regno and registration.subno = charity.subno
	where
	charity.regno = @RegistrationNumber
	and charity.subno = 0
	



CREATE PROCEDURE [CharityData].[FindCharities]
(
	@SearchTerm NVARCHAR(4000),
	@MaximumResults INT
)
AS

/* MAC-210 - Hack to fix a search term that contains AND returning zero results */
SET @SearchTerm = REPLACE(@SearchTerm, ' AND', ' AN');

DECLARE @wildcardedSearch NVARCHAR(4000) = 'FORMSOF(FREETEXT, "' + @SearchTerm + '")';

SELECT TOP (@MaximumResults)
    charity.regno                                   AS 'RegistrationNumber'
   ,TRIM(charity.[Name])                            AS 'Name'
   ,TRIM(charity.add1)                              AS 'Address1'
   ,TRIM(charity.add2)                              AS 'Address2'
   ,TRIM(charity.add3)                              AS 'Address3'
   ,TRIM(charity.add4)                              AS 'Address4'
   ,TRIM(charity.add5)                              AS 'Address5'
   ,TRIM(charity.postcode)                          AS 'PostCode'
   ,reg.regdate                                     AS 'RegistrationDate'
   ,CAST (IIF(charity.orgtype = 'RM', 1, 0) AS BIT) AS 'IsRemoved'
FROM [CharityData].[charity] charity
INNER JOIN [CharityData].[registration] reg on reg.regno = charity.regno and reg.subno = charity.subno
INNER JOIN [CharityData].[charitynamesearch] cns on cns.regno = charity.regno
WHERE CONTAINS(cns.name, @wildcardedSearch)
	

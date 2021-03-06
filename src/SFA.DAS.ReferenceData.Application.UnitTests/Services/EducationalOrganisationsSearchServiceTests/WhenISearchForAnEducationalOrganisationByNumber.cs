﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Application.Services.OrganisationSearch;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Models.Data;
using SFA.DAS.ReferenceData.Types.DTO;

namespace SFA.DAS.ReferenceData.Application.UnitTests.Services.EducationalOrganisationsSearchServiceTests
{
    [TestFixture]
    public class WhenISearchForAnEducationalOrganisationByNumber : EducationalOrganisationSearchServiceTestsBase
    {
        [Test]
        public async Task ThenTheMatchingOrganisationsAreReturned()
        {
            var searchTerm = "98765";
            var maximumResults = 500;

            var expectedOrganisations = new List<EducationOrganisation>
            {
                new EducationOrganisation { AddressLine1  = "2 Line1", AddressLine2 = "2 Line 2", AddressLine3 = "2 Line 3", Town = "2 Line 4", County = "2 Line 5", Name = "2 Name", EducationalType = "ABC999", PostCode = "ZA11AA", URN = 98765 }
            };
            var expectedResults = new PagedResult<EducationOrganisation> { Data = expectedOrganisations, Page = 1, TotalPages = 1 };

            Repository.Setup(x => x.FindOrganisations(searchTerm, maximumResults, 1)).ReturnsAsync(expectedResults);

            var results = (await Service.Search(searchTerm, maximumResults)).ToList();

            Assert.AreEqual(expectedOrganisations.Count, results.Count);
            foreach (var result in results)
            {
                var expectedOrganisation = expectedOrganisations[results.IndexOf(result)];
                Assert.AreEqual(expectedOrganisation.Name, result.Name);
                Assert.AreEqual(expectedOrganisation.AddressLine1, result.Address.Line1);
                Assert.AreEqual(expectedOrganisation.AddressLine2, result.Address.Line2);
                Assert.AreEqual(expectedOrganisation.AddressLine3, result.Address.Line3);
                Assert.AreEqual(expectedOrganisation.Town, result.Address.Line4);
                Assert.AreEqual(expectedOrganisation.County, result.Address.Line5);
                Assert.AreEqual(expectedOrganisation.PostCode, result.Address.Postcode);
                Assert.AreEqual(expectedOrganisation.EducationalType, result.Sector);
                Assert.AreEqual(OrganisationSubType.None, result.SubType);
                Assert.AreEqual(OrganisationType.EducationOrganisation, result.Type);
                Assert.IsNull(result.RegistrationDate);
                Assert.AreEqual(expectedOrganisation.URN.ToString(), result.Code);
            }
        }
    }
}

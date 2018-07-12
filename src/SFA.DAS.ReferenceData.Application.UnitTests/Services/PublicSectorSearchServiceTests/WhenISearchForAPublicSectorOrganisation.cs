using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Api.Client.Dto;
using SFA.DAS.ReferenceData.Application.Services.OrganisationSearch;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Models.Data;
using SFA.DAS.ReferenceData.Types;

namespace SFA.DAS.ReferenceData.Application.UnitTests.Services.PublicSectorSearchServiceTests
{
    [TestFixture]
    public class WhenISearchForAPublicSectorOrganisation
    {
        private Mock<IPublicSectorOrganisationRepository> _repository;
        private PublicSectorSearchService _service;

        [SetUp]
        public void Arrange()
        {
            _repository = new Mock<IPublicSectorOrganisationRepository>();
            _service = new PublicSectorSearchService(_repository.Object);
        }

        [Test]
        public async Task ThenTheMatchingOrganisationsAreReturned()
        {
            var searchTerm = "Skills";
            var maximumResults = 500;

            var expectedOrganisations = new List<PublicSectorOrganisation>
            {
                new PublicSectorOrganisation { AddressLine1  = "Line1", AddressLine2 = "Line 2", AddressLine3 = "Line 3", AddressLine4 = "Line 4", AddressLine5 = "Line 5", Name = "Name", OrganisationCode = "ABC123", Source = DataSource.Nhs, PostCode = "AA11AA", Sector = "Sector"},
                new PublicSectorOrganisation { AddressLine1  = "2 Line1", AddressLine2 = "2 Line 2", AddressLine3 = "2 Line 3", AddressLine4 = "2 Line 4", AddressLine5 = "2 Line 5", Name = "2 Name", OrganisationCode = "ABC999", Source = DataSource.Ons, PostCode = "ZA11AA", Sector = "Sector2"}
            };
            var expectedResults = new PagedResult<PublicSectorOrganisation> { Data = expectedOrganisations, Page = 1, TotalPages = 1 };

            _repository.Setup(x => x.FindOrganisations(searchTerm, maximumResults, 1)).ReturnsAsync(expectedResults);

            var results = (await _service.Search(searchTerm, maximumResults)).ToList();

            Assert.AreEqual(expectedOrganisations.Count, results.Count);
            foreach (var result in results)
            {
                var expectedOrganisation = expectedOrganisations[results.IndexOf(result)];
                Assert.AreEqual(expectedOrganisation.Name, result.Name);
                Assert.AreEqual(expectedOrganisation.AddressLine1, result.Address.Line1);
                Assert.AreEqual(expectedOrganisation.AddressLine2, result.Address.Line2);
                Assert.AreEqual(expectedOrganisation.AddressLine3, result.Address.Line3);
                Assert.AreEqual(expectedOrganisation.AddressLine4, result.Address.Line4);
                Assert.AreEqual(expectedOrganisation.AddressLine5, result.Address.Line5);
                Assert.AreEqual(expectedOrganisation.OrganisationCode, result.Code);
                Assert.AreEqual(expectedOrganisation.PostCode, result.Address.Postcode);
                Assert.AreEqual(expectedOrganisation.Sector, result.Sector);
                Assert.AreEqual(expectedOrganisation.Source.ToString(), result.SubType.ToString());
                Assert.AreEqual(OrganisationType.PublicSector, result.Type);
                Assert.IsNull(result.RegistrationDate);
            }
        }
    }
}

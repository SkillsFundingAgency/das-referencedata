using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Application.Queries.GetPublicOrganisations;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Models;
using SFA.DAS.ReferenceData.Domain.Models.Data;

namespace SFA.DAS.ReferenceData.Application.UnitTests.Queries.GetPublicSectorOrganisations
{
    class WhenIGetPublicSectorOrganisations
    {
        private Mock<IPublicSectorOrganisationRepository> _publicSectorOrganisationRepository;
        private GetPublicSectorOrganisationsHandler _handler;
        private PagedResult<PublicSectorOrganisation> _organisations;
       
        [SetUp]
        public void Arrange()
        {
            _organisations = new PagedResult<PublicSectorOrganisation>
            {
                Data = new List<PublicSectorOrganisation>
                {
                    new PublicSectorOrganisation {Name = "Test Organisation"}
                }
            };

            _publicSectorOrganisationRepository = new Mock<IPublicSectorOrganisationRepository>();

            _publicSectorOrganisationRepository.Setup(x => 
                x.FindOrganisations(
                    It.IsAny<string>(), 
                    It.IsAny<int>(), 
                    It.IsAny<int>())).ReturnsAsync(_organisations);

            _handler = new GetPublicSectorOrganisationsHandler(_publicSectorOrganisationRepository.Object);
        }

        [Test]
        public async Task ThenIShouldGetAllOrganisationsFromTheRepository()
        {
            //Arrange
            var query = new FindPublicSectorOrgainsationQuery
            {
                PageNumber = 2,
                PageSize = 50,
                SearchTerm = "test"
            };


            //Act
            var response = await _handler.Handle(query);

            //Assert
            _publicSectorOrganisationRepository.Verify(x => x.FindOrganisations(query.SearchTerm, query.PageSize, query.PageNumber), Times.Once);
            Assert.AreEqual(_organisations.Data, response.Organisations.Data);
        }
    }
}

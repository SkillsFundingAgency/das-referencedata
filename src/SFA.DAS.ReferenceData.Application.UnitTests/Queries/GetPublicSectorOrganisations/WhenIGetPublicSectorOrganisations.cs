using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Application.Queries.GetPublicOrganisations;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.Application.UnitTests.Queries.GetPublicSectorOrganisations
{
    class WhenIGetPublicSectorOrganisations
    {
        private Mock<IPublicSectorOrganisationLookUpService> _lookUpService;
        private GetPublicSectorOrganisationsHandler _handler;
        private ICollection<PublicSectorOrganisation> _organisations;

        [SetUp]
        public void Arrange()
        {
            _organisations = new List<PublicSectorOrganisation>
            {
                new PublicSectorOrganisation {Name = "Test Organisation"}
            };

            _lookUpService = new Mock<IPublicSectorOrganisationLookUpService>();

            _lookUpService.Setup(x => x.GetOrganisations())
                          .ReturnsAsync(_organisations);

            _handler = new GetPublicSectorOrganisationsHandler(_lookUpService.Object);
        }

        [Test]
        public async Task ThenIShouldGetAllOrganisationsFromTheRepository()
        {
            //Act
            var response = await _handler.Handle(new GetPublicSectorOrgainsationsQuery());

            //Assert
            _lookUpService.Verify(x => x.GetOrganisations(), Times.Once);
            Assert.AreEqual(_organisations, response.Organisations);
        }
    }
}

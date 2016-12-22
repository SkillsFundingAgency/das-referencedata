using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Application.Queries.GetPublicOrganisations;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.Application.UnitTests.Queries.GetPublicSectorOrganisations
{
    class WhenIGetPublicSectorOrganisations
    {
        private Mock<IPublicSectorOrganisationRepository> _publicSectorOrganisationRepository;
        private GetPublicSectorOrganisationsHandler _handler;
        private ICollection<PublicSectorOrganisation> _organisations;

        [SetUp]
        public void Arrange()
        {
            _organisations = new List<PublicSectorOrganisation>
            {
                new PublicSectorOrganisation {Name = "Test Organisation"}
            };

            _publicSectorOrganisationRepository = new Mock<IPublicSectorOrganisationRepository>();

            _publicSectorOrganisationRepository.Setup(x => 
                x.FindOrganisations(
                    It.IsAny<string>(), 
                    It.IsAny<int>(), 
                    It.IsAny<int>())).ReturnsAsync(_organisations);

            _handler = new GetPublicSectorOrganisationsHandler(_publicSectorOrganisationRepository.Object);
        }

        //[Test]
        //public async Task ThenIShouldGetAllOrganisationsFromTheRepository()
        //{
        //    //Act
        //    var response = await _handler.Handle(new FindPublicSectorOrgainsationQuery());

        //    //Assert
        //    _publicSectorOrganisationRepository.Verify(x => x.GetOrganisations(), Times.Once);
        //    Assert.AreEqual(_organisations, response.Organisations);
        //}
    }
}

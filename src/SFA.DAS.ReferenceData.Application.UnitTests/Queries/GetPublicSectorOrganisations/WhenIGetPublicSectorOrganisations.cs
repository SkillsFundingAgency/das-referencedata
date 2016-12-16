using System.Collections.Generic;
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
        private Mock<IOrganisationRepository> _repository;
        private GetPublicSectorOrganisationsHandler _handler;
        private ICollection<PublicSectorOrganisation> _organisations;

        [SetUp]
        public void Arrange()
        {
            _organisations = new List<PublicSectorOrganisation>
            {
                new PublicSectorOrganisation {Name = "Test Organisation"}
            };

            _repository = new Mock<IOrganisationRepository>();

            _repository.Setup(x => x.GetPublicSectorOrganisations())
                       .ReturnsAsync(_organisations);

            _handler = new GetPublicSectorOrganisationsHandler(_repository.Object);
        }

        [Test]
        public async Task ThenIShouldGetAllOrganisationsFromTheRepository()
        {
            //Act
            var response = await _handler.Handle(new GetPublicSectorOrgainsationsQuery());

            //Assert
            _repository.Verify(x => x.GetPublicSectorOrganisations(), Times.Once);
            Assert.AreEqual(_organisations, response.Organisations);
        }
    }
}

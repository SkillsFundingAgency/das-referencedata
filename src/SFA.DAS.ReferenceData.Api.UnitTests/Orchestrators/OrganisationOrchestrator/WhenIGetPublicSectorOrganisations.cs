using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Application.Queries.GetPublicOrganisations;
using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.Api.UnitTests.Orchestrators.OrganisationOrchestrator
{
    class WhenIGetPublicSectorOrganisations
    {
        private Mock<IMediator> _mediator;
        private Api.Orchestrators.OrganisationOrchestrator _orchestrator;
        private GetPublicSectorOrganisationsResponse _response;

        [SetUp]
        public void Arrange()
        {
            _response = new GetPublicSectorOrganisationsResponse
            {
                Organisations = new List<PublicSectorOrganisation>
                {
                    new PublicSectorOrganisation{ Name = "Test Organisation"}
                }
            };

            _mediator = new Mock<IMediator>();

            _mediator.Setup(x => x.SendAsync(It.IsAny<GetPublicSectorOrgainsationsQuery>()))
                .ReturnsAsync(_response);

            _orchestrator = new Api.Orchestrators.OrganisationOrchestrator(_mediator.Object);
        }

        [Test]
        public async Task ThenIShouldReturnCorrectOrganisations()
        {
            //Act
            var result = await _orchestrator.GetPublicSectorOrganisations();
            
            //Assert
            _mediator.Verify(x => x.SendAsync(It.IsAny<GetPublicSectorOrgainsationsQuery>()), Times.Once);
            Assert.AreEqual(_response.Organisations, result.Data);
            Assert.AreEqual(HttpStatusCode.OK, result.Status);
        }
    }
}

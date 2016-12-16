using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Results;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Api.Controllers;
using SFA.DAS.ReferenceData.Application.Queries.GetPublicOrganisations;
using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.Api.UnitTests.Controllers.OrganisationControllerTests
{
    class WhenIGetPublicSectorOrganisations
    {
        private Mock<IMediator> _mediator;
       
        private GetPublicSectorOrganisationsResponse _response;
        private OrganisationController _controller;

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

            _controller = new OrganisationController(_mediator.Object);
        }

        [Test]
        public async Task ThenIShouldReturnCorrectOrganisations()
        {
            //Act
            var result = await _controller.GetPublicSectorOrganisations() 
                as OkNegotiatedContentResult<ICollection<PublicSectorOrganisation>>;
            
            //Assert
            _mediator.Verify(x => x.SendAsync(It.IsAny<GetPublicSectorOrgainsationsQuery>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(_response.Organisations, result.Content);
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Api.Client.Dto;
using SFA.DAS.ReferenceData.Api.Controllers;
using SFA.DAS.ReferenceData.Application.Queries.GetPublicOrganisations;
using SFA.DAS.ReferenceData.Types;
using SFA.DAS.ReferenceData.Types.DTO;

namespace SFA.DAS.ReferenceData.Api.UnitTests.Controllers.OrganisationControllerTests
{
    class WhenIGetPublicSectorOrganisations
    {
        private Mock<IMediator> _mediator;
       
        private FindPublicSectorOrganisationResponse _response;
        private OrganisationController _controller;

        [SetUp]
        public void Arrange()
        {
            _response = new FindPublicSectorOrganisationResponse
            {
                Organisations = new PagedApiResponse<PublicSectorOrganisation>
                {
                    Data = new List<PublicSectorOrganisation> { new PublicSectorOrganisation{ Name = "Test Organisation"}}
                }
            };

            _mediator = new Mock<IMediator>();

            _mediator.Setup(x => x.SendAsync(It.IsAny<FindPublicSectorOrgainsationQuery>()))
                     .ReturnsAsync(_response);

            _controller = new OrganisationController(_mediator.Object, Mock.Of<ILog>());
        }

        [Test]
        public async Task ThenIShouldReturnCorrectOrganisations()
        {
            //Act
            var result = await _controller.GetPublicSectorOrganisations() 
                as OkNegotiatedContentResult<PagedApiResponse<PublicSectorOrganisation>>;
            
            //Assert
            _mediator.Verify(x => x.SendAsync(It.IsAny<FindPublicSectorOrgainsationQuery>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(_response.Organisations, result.Content);
        }
    }
}

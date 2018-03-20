using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Api.Client.Dto;
using SFA.DAS.ReferenceData.Api.Controllers;
using SFA.DAS.ReferenceData.Application.Queries.GetEducationalOrganisations;
using EducationOrganisation = SFA.DAS.ReferenceData.Domain.Models.Education.EducationOrganisation;

namespace SFA.DAS.ReferenceData.Api.UnitTests.Controllers.OrganisationControllerTests
{
    internal class WhenIGetEducationalOrganisations
    {
        private Mock<IMediator> _mediator;
       
        private FindEducationalOrganisationsResponse _response;
        private OrganisationController _controller;

        [SetUp]
        public void Arrange()
        {
            _response = new FindEducationalOrganisationsResponse
            {
                Organisations = new PagedApiResponse<EducationOrganisation>
                {
                    Data = new List<EducationOrganisation> { new EducationOrganisation { Name = "Test Organisation"}}
                }
            };

            _mediator = new Mock<IMediator>();

            _mediator.Setup(x => x.SendAsync(It.IsAny<FindEducationalOrganisationsQuery>()))
                     .ReturnsAsync(_response);

            _controller = new OrganisationController(_mediator.Object, Mock.Of<ILog>());
        }

        [Test]
        public async Task ThenIShouldReturnCorrectOrganisations()
        {
            //Act
            var result = await _controller.GetEducaltionalOrganisation() 
                as OkNegotiatedContentResult<PagedApiResponse<EducationOrganisation>>;
            
            //Assert
            _mediator.Verify(x => x.SendAsync(It.IsAny<FindEducationalOrganisationsQuery>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(_response.Organisations, result.Content);
        }

        [Test]
        public async Task ThenIShouldReturnNotFoundStatusIfNoOrganisationsFound()
        {
            //Arrange
            _mediator.Setup(x => x.SendAsync(It.IsAny<FindEducationalOrganisationsQuery>()))
                .ReturnsAsync(new FindEducationalOrganisationsResponse
                {
                    Organisations = new PagedApiResponse<EducationOrganisation>
                    {
                        Data = new List<EducationOrganisation>()
                    }
                });


            //Act
            var result = await _controller.GetEducaltionalOrganisation() as NotFoundResult;

            //Assert
            Assert.IsNotNull(result);
        }
    }
}

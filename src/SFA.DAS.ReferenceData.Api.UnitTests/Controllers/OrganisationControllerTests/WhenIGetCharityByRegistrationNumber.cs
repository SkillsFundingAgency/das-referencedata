using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Api.Controllers;
using SFA.DAS.ReferenceData.Application.Queries.GetCharityByRegistrationNumber;
using SFA.DAS.ReferenceData.Application.Queries.GetPublicOrganisations;
using SFA.DAS.ReferenceData.Domain.Models;
using SFA.DAS.ReferenceData.Domain.Models.Charity;

namespace SFA.DAS.ReferenceData.Api.UnitTests.Controllers.OrganisationControllerTests
{
    class WhenIGetCharityByRegistrationNumber
    {
        private Mock<IMediator> _mediator;

        private GetCharityByRegistrationNumberResponse _response;
        private OrganisationController _controller;

        [SetUp]
        public void Arrange()
        {
            _response = new GetCharityByRegistrationNumberResponse
            {
                Charity = new Charity
                {
                    RegistrationNumber = 123,
                    Name = "Test Charity"
                }
            };

            _mediator = new Mock<IMediator>();

            _mediator.Setup(x => x.SendAsync(It.Is<GetCharityByRegistrationNumberQuery>(query => query.RegistrationNumber == 123)))
                .ReturnsAsync(_response);

            _mediator.Setup(x => x.SendAsync(It.Is<GetCharityByRegistrationNumberQuery>(query => query.RegistrationNumber == 456)))
                 .ReturnsAsync(new GetCharityByRegistrationNumberResponse
                 {
                     Charity = null
                 });

            _controller = new OrganisationController(_mediator.Object);
        }

        [Test]
        public async Task ThenIShouldReturnTheCorrectCharity()
        {
            //Act
            var result = await _controller.GetCharity(123) as OkNegotiatedContentResult<Charity>;

            //Assert
            _mediator.Verify(x => x.SendAsync(It.IsAny<GetCharityByRegistrationNumberQuery>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(_response.Charity, result.Content);
        }

        [Test]
        public async Task ThenIShouldReceiveANotFoundResponseIfTheCharityIsNotFound()
        {
            //Act
            var result = await _controller.GetCharity(456) as NotFoundResult;

            //Assert
            _mediator.Verify(x => x.SendAsync(It.IsAny<GetCharityByRegistrationNumberQuery>()), Times.Once);
            Assert.IsNotNull(result);
        }
    }
}

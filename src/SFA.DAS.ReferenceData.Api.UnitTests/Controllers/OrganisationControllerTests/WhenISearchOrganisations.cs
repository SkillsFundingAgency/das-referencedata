using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Api.Client.Dto;
using SFA.DAS.ReferenceData.Api.Controllers;
using SFA.DAS.ReferenceData.Application.Queries.SearchOrganisations;
using SFA.DAS.ReferenceData.Types;

namespace SFA.DAS.ReferenceData.Api.UnitTests.Controllers.OrganisationControllerTests
{
    class WhenISearchOrganisations
    {
        private Mock<IMediator> _mediator;
        private OrganisationController _controller;

        [SetUp]
        public void Arrange()
        {
            _mediator = new Mock<IMediator>();
            _controller = new OrganisationController(_mediator.Object, Mock.Of<ILog>());
        }

        [Test]
        public async Task ThenIShouldReturnCorrectOrganisations()
        {
            var response = new SearchOrganisationsResponse
            {
                Organisations = new List<Organisation>
                {
                    new Organisation ()
                }
            };

            var searchTerm = "Test";
            var maximumResults = 500;

            _mediator.Setup(x => x.SendAsync(It.Is<SearchOrganisationsQuery>(y => y.SearchTerm == searchTerm && y.MaximumResults == maximumResults))).ReturnsAsync(response);

            //Act
            var result = await _controller.SearchOrganisations(searchTerm, maximumResults) as OkNegotiatedContentResult<IEnumerable<Organisation>>;
            
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(response.Organisations, result.Content);
        }

        [Test]
        public async Task ThenIShouldReturnZeroOrganisationsAfterException()
        {
            var searchTerm = "Test";
            var maximumResults = 500;

            _mediator.Setup(x => 
                x.SendAsync(It.Is<SearchOrganisationsQuery>(y => 
                    y.SearchTerm == searchTerm && y.MaximumResults == maximumResults)))
                    .Throws<Exception>();

            //Act
            var result = await _controller.SearchOrganisations(searchTerm, maximumResults) as OkNegotiatedContentResult<IEnumerable<Organisation>>;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Content.Any());

        }
    }
}

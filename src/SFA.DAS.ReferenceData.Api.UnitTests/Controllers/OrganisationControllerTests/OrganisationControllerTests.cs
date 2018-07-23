using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Common.Domain.Types;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Api.Controllers;
using SFA.DAS.ReferenceData.Application.Queries.GetOrganisation;
using SFA.DAS.ReferenceData.Types.DTO;
using SFA.DAS.ReferenceData.Types.Exceptions;

namespace SFA.DAS.ReferenceData.Api.UnitTests.Controllers.OrganisationControllerTests
{
    class OrganisationControllerTests
    {
        [Test]
        public Task Get_BadOrganisationIdentifierExeption_ShouldReturnBadRequest()
        {
            return CheckExceptionTranslatesIntoStatusCode<BadOrganisationIdentifierExeption>(HttpStatusCode.BadRequest);
        }

        [Test]
        public Task Get_OperationNotSupportedForOrganisationType_ShouldReturnBadRequest()
        {
            return CheckExceptionTranslatesIntoStatusCode<OperationNotSupportedForOrganisationTypeException>(HttpStatusCode.BadRequest);
        }

        [Test]
        public Task Get_OrganisationNotFoundExeption_ShouldReturnNotFound()
        {
            return CheckExceptionTranslatesIntoStatusCode<BadOrganisationIdentifierExeption>(HttpStatusCode.BadRequest);
        }

        [Test]
        public Task Get_AnyOtherException_ShouldReturnInternalServerError()
        {
            return CheckExceptionTranslatesIntoStatusCode<Exception>(HttpStatusCode.InternalServerError);
        }

        [Test]
        public Task Get_OrganisationIsFoundOkay_ShouldReturnOkay()
        {
            return CheckSuccessfulCallReturnsExpectedStatus(HttpStatusCode.OK);
        }

        private async Task CheckExceptionTranslatesIntoStatusCode<TException>(HttpStatusCode expectedStatusCode) where TException : Exception, new()
        {
            var fixtures = new OrganisationControllerTestFixtures()
                .SetQueryException<TException>();

            var result = await fixtures.CallGetAsync("123", OrganisationType.CompaniesHouse);

            Assert.AreEqual(expectedStatusCode, result.StatusCode);
        }

        private async Task CheckSuccessfulCallReturnsExpectedStatus(HttpStatusCode expectedStatusCode)
        {
            const string registeredId = "123";
            const OrganisationType organisationType = OrganisationType.CompaniesHouse;

            var organisation = new Organisation
            {
                Type = organisationType,
                Code = registeredId
            };

            var fixtures = new OrganisationControllerTestFixtures()
                .SetQueryResult(organisation);

            var result = await fixtures.CallGetAsync(registeredId, organisationType);

            Assert.AreEqual(expectedStatusCode, result.StatusCode);
        }
    }

    internal class OrganisationControllerTestFixtures
    {
        public OrganisationControllerTestFixtures()
        {
            MediatorMock = new Mock<IMediator>();
            LogMock = new Mock<ILog>();
        }

        public Mock<IMediator> MediatorMock { get; }
        public IMediator Mediator => MediatorMock.Object;
        public Mock<ILog> LogMock { get; }
        public ILog Log => LogMock.Object;

        public OrganisationControllerTestFixtures SetQueryException<TException>() where TException: Exception, new()
        {
            MediatorMock
                .Setup(m => m.SendAsync(It.IsAny<GetOrganisationQuery>()))
                .Throws<TException>();

            return this;
        }

        public OrganisationControllerTestFixtures SetQueryResult(Organisation organisation)
        {
            var response = new GetOrganisationResponse
            {
                Organisation = organisation
            };

            MediatorMock
                .Setup(m => m.SendAsync(It.Is<GetOrganisationQuery>(goq => goq.OrganisationType == organisation.Type && goq.Identifier == organisation.Code)))
                .ReturnsAsync(() => response);

            return this;
        }

        public async Task<HttpResponseMessage> CallGetAsync(string identifier, OrganisationType organisationType)
        {
            var controller = CreateOrganisationController();
            
            var cont = await controller.Get(identifier, organisationType);

            return await cont.ExecuteAsync(CancellationToken.None);
        }

        public OrganisationController CreateOrganisationController()
        {
            return new OrganisationController(Mediator, Log)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }
    }
}

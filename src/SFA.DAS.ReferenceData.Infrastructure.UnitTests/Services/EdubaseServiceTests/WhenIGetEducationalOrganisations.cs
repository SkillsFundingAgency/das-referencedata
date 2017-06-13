using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dfe.Edubase2.SoapApi.Client;
using Dfe.Edubase2.SoapApi.Client.EdubaseService;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Infrastructure.Factories;
using SFA.DAS.ReferenceData.Infrastructure.Services;

namespace SFA.DAS.ReferenceData.Infrastructure.UnitTests.Services.EdubaseServiceTests
{
    internal class WhenIGetEducationalOrganisations
    {
        private EdubaseService _service;
        private Mock<IEdubaseClientFactory> _clientFactory;
        private Mock<IEstablishmentClient> _client;
        private Mock<ILogger> _logger;

        [SetUp]
        public void Arrange()
        {
            _clientFactory = new Mock<IEdubaseClientFactory>();
            _client = new Mock<IEstablishmentClient>();
            _logger = new Mock<ILogger>();
            _service = new EdubaseService(_clientFactory.Object, _logger.Object);

            _clientFactory.Setup(x => x.Create()).Returns(_client.Object);

            _client.Setup(x => x.FindEstablishmentsAsync(It.IsAny<EstablishmentFilter>()))
                .ReturnsAsync(new List<Establishment>());
        }

        [Test]
        public async Task ThenIShouldSearchForOrganisations()
        {
            //Act
            await _service.GetOrganisations();

            //Assert
            _client.Verify(x => x.FindEstablishmentsAsync(It.IsAny<EstablishmentFilter>()), Times.Once);
        }

        [Test]
        public async Task ThenIShouldSearchForOrganisationNames()
        {
            //Act
            await _service.GetOrganisations();

            //Assert
            _client.Verify(x => x.FindEstablishmentsAsync(
                It.Is<EstablishmentFilter>(f => f.Fields.Contains("EstablishmentName"))), Times.Once);
        }

        [Test]
        public async Task ThenIShouldReturnFoundOrgainsations()
        {
            //Arrange
            var establishment = new Establishment
            {
                EstablishmentName = "Test Establishment"
            };

            _client.Setup(x => x.FindEstablishmentsAsync(It.IsAny<EstablishmentFilter>()))
                .ReturnsAsync(new List<Establishment> {establishment});

            //Act
            var result = await _service.GetOrganisations();

            //Assert
            Assert.AreEqual(establishment.EstablishmentName, result.First().Name);
        }

        [Test]
        public async Task ThenIShouldReturnEmptyListIfErrorOccurs()
        {
            //Arrange
            _client.Setup(x => x.FindEstablishmentsAsync(It.IsAny<EstablishmentFilter>()))
                .Throws<WebException>();

            //Act
            var result = await _service.GetOrganisations();

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task ThenIShouldLogWhenErrorsOccur()
        {
            //Arrange
            var exception = new WebException();
            _client.Setup(x => x.FindEstablishmentsAsync(It.IsAny<EstablishmentFilter>()))
                .Throws(exception);

            //Act
            await _service.GetOrganisations();

            //Assert
            _logger.Verify(x => x.Error(exception, It.IsAny<string>()), Times.Once);
        }
    }
}

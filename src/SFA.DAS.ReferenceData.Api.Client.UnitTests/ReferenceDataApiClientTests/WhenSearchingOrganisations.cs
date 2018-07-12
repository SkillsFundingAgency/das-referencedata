using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Api.Client.Dto;
using SFA.DAS.ReferenceData.Types;

namespace SFA.DAS.ReferenceData.Api.Client.UnitTests.ReferenceDataApiClientTests
{
    public class WhenSearchingOrganisations
    {
        private Mock<IReferenceDataApiConfiguration> _configuration;
        private Mock<SecureHttpClient> _httpClient;
        private ReferenceDataApiClient _apiClient;

        [SetUp]
        public void Arrange()
        {
            _configuration = new Mock<IReferenceDataApiConfiguration>();
            _configuration.SetupGet(x => x.ApiBaseUrl).Returns("http://some-url/api/organisations");

            _httpClient = new Mock<SecureHttpClient>();
            _apiClient = new ReferenceDataApiClient(_configuration.Object, _httpClient.Object);
        }

        [Test]
        public async Task ThenOrganisationsShouldBeReturned()
        {
            //Arrange
            var data = new List<Organisation> { new Organisation() };
            var expectedUrl = "http://some-url/api/organisations/?searchTerm=test&maximumResults=100";
            _httpClient.Setup(c => c.GetAsync(expectedUrl, false)).ReturnsAsync(() => JsonConvert.SerializeObject(data));

            //Act
            var result = await _apiClient.SearchOrganisations("test", 100);

            //Assert
            _httpClient.Verify(x => x.GetAsync(expectedUrl, false), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<Organisation>>(result);
        }

        [Test]
        public async Task AndNoResultsAreFoundThenNothingIsReturned()
        {
            var expectedUrl = "http://some-url/api/organisations/?searchTerm=test&maximumResults=100";
            _httpClient.Setup(c => c.GetAsync(expectedUrl, false)).ReturnsAsync(() => null);

            //Act
            var result = await _apiClient.SearchOrganisations("test", 100);

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task ShouldEncodedSearchTerm()
        {
            var expectedUrl = "http://some-url/api/organisations/?searchTerm=test%20query&maximumResults=100";

            //Act
            var result = await _apiClient.SearchOrganisations("test query", 100);

            //Assert
            _httpClient.Verify(c => c.GetAsync(expectedUrl, false), Times.Once);
        }
    }
}

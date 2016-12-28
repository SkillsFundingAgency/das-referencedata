using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace SFA.DAS.ReferenceData.Api.Client.UnitTests.ReferenceDataApiClientTests
{
    public class WhenGettingACharity
    {
        private Mock<IReferenceDataApiConfiguration> _configuration;
        private Mock<SecureHttpClient> _httpClient;
        private ReferenceDataApiClient _apiClient;

        [SetUp]
        public void Arrange()
        {
            _configuration = new Mock<IReferenceDataApiConfiguration>();
            _configuration.SetupGet(x => x.ApiBaseUrl).Returns("http://some-url/api/organisations");

            var data = new Dto.Charity
            {
                RegistrationNumber = 123,
                Name = "Test Charity"
            };

            _httpClient = new Mock<SecureHttpClient>();
            _httpClient.Setup(c => c.GetAsync(It.Is<string>(s => s == @"http://some-url/api/organisations/charities/123"))).ReturnsAsync(() => JsonConvert.SerializeObject(data));
            _apiClient = new ReferenceDataApiClient(_configuration.Object, _httpClient.Object);
        }

        [Test]
        public async Task ThenItShouldCallTheApiWithTheCorrectUrl()
        {
            //Act
            await _apiClient.GetCharity(123);

            //Assert
            var expectedUrl = $"http://some-url/api/organisations/charities/123";
            _httpClient.Verify(x=> x.GetAsync(expectedUrl), Times.Once);
        }

        [Test]
        public async Task ThenIShouldGetACharity()
        {
            //Act
            var actual = await _apiClient.GetCharity(123);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<Dto.Charity>(actual);
        }
    }
}

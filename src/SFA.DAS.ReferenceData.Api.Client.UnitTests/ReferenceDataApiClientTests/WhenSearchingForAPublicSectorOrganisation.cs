using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Api.Client.Dto;

namespace SFA.DAS.ReferenceData.Api.Client.UnitTests.ReferenceDataApiClientTests
{
    public class WhenSearchingForAPublicSectorOrganisation
    {
        private Mock<IReferenceDataApiConfiguration> _configuration;
        private Mock<SecureHttpClient> _httpClient;
        private ReferenceDataApiClient _apiClient;
        private PagedApiResponse<PublicSectorOrganisation> _data;

        [SetUp]
        public void Arrange()
        {
            _configuration = new Mock<IReferenceDataApiConfiguration>();
            _configuration.SetupGet(x => x.ApiBaseUrl).Returns("http://some-url/api/organisations");

            _data = new PagedApiResponse<PublicSectorOrganisation>
            {
                Data = new EditableList<PublicSectorOrganisation>
                {
                    new PublicSectorOrganisation
                    {
                        Name = "Test Public Sector"
                    }
                }
            };

                                                
            _httpClient = new Mock<SecureHttpClient>();
            _httpClient.Setup(c => c.GetAsync(It.Is<string>(s => s == @"http://some-url/api/organisations/publicsectorbodies?searchTerm=test&pageNumber=0&pageSize=100"), true)).ReturnsAsync(() => JsonConvert.SerializeObject(_data));
            _apiClient = new ReferenceDataApiClient(_configuration.Object, _httpClient.Object);
        }

        [Test]
        public async Task ThenItShouldCallTheApiWithTheCorrectUrl()
        {
            //Act
            await _apiClient.SearchPublicSectorOrganisation("test", 0, 100);

            //Assert
            var expectedUrl = $"http://some-url/api/organisations/publicsectorbodies?searchTerm=test&pageNumber=0&pageSize=100";
            _httpClient.Verify(x => x.GetAsync(expectedUrl, true), Times.Once);
        }

        [Test]
        public async Task ThenIShouldGetAPagedResponseOfPublicSectorOrganisations()
        {
            //Act
            var actual = await _apiClient.SearchPublicSectorOrganisation("test", 0, 100);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<Dto.PagedApiResponse<PublicSectorOrganisation>>(actual);
        }

        [Test]
        public async Task ShouldCallTheApiWithTheCorrectEncodedUrl()
        {
            //Arrange
            _httpClient.Setup(c => c.GetAsync(It.IsAny<string>(), true)).ReturnsAsync(() => JsonConvert.SerializeObject(_data));

            //Act
            await _apiClient.SearchPublicSectorOrganisation("test query", 0, 100);

            //Assert
            var expectedUrl = $"http://some-url/api/organisations/publicsectorbodies?searchTerm=test%20query&pageNumber=0&pageSize=100";
            _httpClient.Verify(x => x.GetAsync(expectedUrl, true), Times.Once);
        }

    }
}

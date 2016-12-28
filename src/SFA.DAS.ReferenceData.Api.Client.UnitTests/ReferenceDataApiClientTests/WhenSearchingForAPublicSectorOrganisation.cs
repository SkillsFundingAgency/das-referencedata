using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        [SetUp]
        public void Arrange()
        {
            _configuration = new Mock<IReferenceDataApiConfiguration>();
            _configuration.SetupGet(x => x.ApiBaseUrl).Returns("http://some-url/api/organisations");

            var data = new PagedApiResponse<PublicSectorOrganisation>
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
            _httpClient.Setup(c => c.GetAsync(It.Is<string>(s => s == @"http://some-url/api/organisations/publicsectorbodies?searchTerm=test&pageNumber=0&pageSize=100"))).ReturnsAsync(() => JsonConvert.SerializeObject(data));
            _apiClient = new ReferenceDataApiClient(_configuration.Object, _httpClient.Object);
        }

        [Test]
        public async Task ThenItShouldCallTheApiWithTheCorrectUrl()
        {
            //Act
            await _apiClient.SearchPublicSectorOrganisation("test", 0, 100);

            //Assert
            var expectedUrl = $"http://some-url/api/organisations/publicsectorbodies?searchTerm=test&pageNumber=0&pageSize=100";
            _httpClient.Verify(x => x.GetAsync(expectedUrl), Times.Once);
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
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Domain.Configuration;
using SFA.DAS.ReferenceData.Domain.Http;
using SFA.DAS.ReferenceData.Domain.Models;
using SFA.DAS.ReferenceData.Infrastructure.Responses;
using SFA.DAS.ReferenceData.Infrastructure.Services;
using SFA.DAS.ReferenceData.Types.DTO;

namespace SFA.DAS.ReferenceData.Infrastructure.UnitTests.Services.PoliceServiceTests
{
    public class WhenRetrievingPoliceData
    {
        private PoliceDataLookupService _policeDataLookupService;        
        private Mock<IHttpClientWrapper> _httpClientWrapper;
        private ReferenceDataApiConfiguration _configuration = new ReferenceDataApiConfiguration
        {
            PoliceForcesUrl = "https://police-url.something"
        };
        private string _capturedBaseUrl;

        [SetUp]
        public void Arrange()
        {
            _httpClientWrapper = new Mock<IHttpClientWrapper>();

            _httpClientWrapper
                .SetupSet(w => w.BaseUrl = It.IsAny<string>())
                .Callback<string>(url => _capturedBaseUrl = url);
            
            _policeDataLookupService = new PoliceDataLookupService(_httpClientWrapper.Object, _configuration);
        }

        [Test]
        public async Task ThenCallPoliceApi()
        {
            //Act
            _ = await _policeDataLookupService.GetGbPoliceForces();

            //Assert            
            _httpClientWrapper.Verify(m => m.Get<List<PoliceApiResponse>>(string.Empty, string.Empty));
            _capturedBaseUrl.Should().Be(_configuration.PoliceForcesUrl);
        }

        [Test]
        public async Task ThenReturnMappedOrganisationLookupData()
        {
            // Arrange
            _httpClientWrapper
                .Setup(m => m.Get<List<PoliceApiResponse>>(string.Empty, string.Empty))
                .ReturnsAsync(new List<PoliceApiResponse>
                {
                    new PoliceApiResponse
                    {
                        Name = "Police_Force_A"
                    },
                    new PoliceApiResponse
                    {
                        Name = "Police_Force_B"
                    }
                });

            var expectedResult = new PublicSectorOrganisationLookUp
            {
                Organisations = new List<PublicSectorOrganisation>
                {
                    new PublicSectorOrganisation
                    {
                        Name = "Police_Force_A",
                        Source = DataSource.Police
                    },
                    new PublicSectorOrganisation
                    {
                        Name = "Police_Force_B",
                        Source = DataSource.Police
                    }
                }
            };
                
            //Act
            var result = await _policeDataLookupService.GetGbPoliceForces();

            //Assert            
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
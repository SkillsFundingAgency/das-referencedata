using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Api.Client.Dto;
using SFA.DAS.ReferenceData.Types.DTO;
using SFA.DAS.ReferenceData.Types.Exceptions;

namespace SFA.DAS.ReferenceData.Api.Client.UnitTests
{
    public class ReferenceDataApiClientTests 
    {
        const string searchTerm = "test query";
        const string complexSearchTerm = "Test & Query < &";
        const int startPage = 0;
        const int pageSize = 100;
        private int maximumResults = 500;

        [Test]
        public Task GetCharity_ExistingCharity_ShouldReturnExpectedCharity()
        {
            const int charityNumber = 123;
            var expectedCharity = new Charity {RegistrationNumber = charityNumber };

            return new ReferenceDataApiClientTestFixtures()
                .WithBaseUrl(@"http://some-url/api/organisations/")
                .WithCharityResponse(charityNumber, expectedCharity)
                .CheckGetCharityAsync(charityNumber, actualCharity => Assert.AreEqual(expectedCharity.RegistrationNumber, actualCharity.RegistrationNumber));
        }

        [Test]
        public async Task GetCharity_ExistingCharityOrNot_ShouldCallExpectedUrl()
        {
            const int charityNumber = 123;
            var expectedCharity = new Charity { RegistrationNumber = charityNumber };

            var fixtures = new ReferenceDataApiClientTestFixtures()
                .WithBaseUrl(@"http://some-url/api/organisations/")
                .WithCharityResponse(charityNumber, expectedCharity);

            await fixtures.GetCharityAsync(charityNumber);

            fixtures.ChackUrlCalled($"{fixtures.BaseUrl}charities/{charityNumber}");
        }

        [TestCase(OrganisationType.Company, "123", HttpStatusCode.NotFound, typeof(OrganisationNotFoundExeption))]
        [TestCase(OrganisationType.Company, "123", HttpStatusCode.BadRequest, typeof(InvalidGetOrganisationRequest))]
        public void GetLatestDetails_SpecifiedStatusCode_ShouldCauseExpectedExceptionInClient(
            OrganisationType organisationType, 
            string registeredId, 
            HttpStatusCode statusCode,
            Type expectedException)
        {
            var expectedOrganisation = new Organisation { Code = registeredId};

            var fixtures = new ReferenceDataApiClientTestFixtures()
                .WithBaseUrl(@"http://some-url/api/organisations/")
                .WithGetLatestDetailsResponse(organisationType, registeredId, expectedOrganisation, statusCode);

            Assert.ThrowsAsync(expectedException, () => fixtures.GetLatestDetailsAsync(organisationType, registeredId));
        }

        [Test]
        public void SearchPublicSectorOrganisation_WithValidParams_ShouldCallTheApiWithTheCorrectUrl()
        {
            var fixtures = new ReferenceDataApiClientTestFixtures()
                .WithBaseUrl(@"http://some-url/api/organisations/")
                .WithSearchPublicSectorResult(searchTerm, startPage, pageSize, new PublicSectorOrganisation());

            fixtures.GetSearchPublicSectorAsync(searchTerm);

            fixtures.ChackUrlCalled(fixtures.BuildSearchPublicSectorUrl(searchTerm, startPage, pageSize));
        }

        [Test]
        public void SearchPublicSectorOrganisation_WithTrickyParams_ShouldEncodeParamsCorrectly()
        {
            var fixtures = new ReferenceDataApiClientTestFixtures()
                .WithBaseUrl(@"http://some-url/api/organisations/")
                .WithSearchPublicSectorResult(complexSearchTerm, startPage, pageSize, new PublicSectorOrganisation());

            fixtures.GetSearchPublicSectorAsync(complexSearchTerm);

            fixtures.ChackUrlCalled(fixtures.BuildSearchPublicSectorUrl(complexSearchTerm, startPage, pageSize));
        }


        [Test]
        public void SearchOrganisations_WithValidParams_ShouldCallTheApiWithTheCorrectUrl()
        {
            var fixtures = new ReferenceDataApiClientTestFixtures()
                .WithBaseUrl(@"http://some-url/api/organisations/")
                .WithSearchOrganisationsResult(searchTerm, maximumResults, new Organisation());

            fixtures.GetOrganisationSearch(searchTerm, maximumResults);

            fixtures.ChackUrlCalled(fixtures.BuildSearchOrganisationUrl(searchTerm, maximumResults));
        }

        [Test]
        public void SearchOrganisations_WithTrickyParams_ShouldEncodeParamsCorrectly()
        {
            var fixtures = new ReferenceDataApiClientTestFixtures()
                .WithBaseUrl(@"http://some-url/api/organisations/")
                .WithSearchOrganisationsResult(complexSearchTerm, maximumResults, new Organisation());

            fixtures.GetOrganisationSearch(complexSearchTerm, maximumResults);

            fixtures.ChackUrlCalled(fixtures.BuildSearchOrganisationUrl(complexSearchTerm, maximumResults));
        }
    }

    internal class ReferenceDataApiClientTestFixtures
    {
        public ReferenceDataApiClientTestFixtures()
        {
            ConfigurationMock = new Mock<IReferenceDataApiConfiguration>();
            HttpClientMock = new Mock<SecureHttpClient>();
            CalledUrls = new List<string>();
        }

        public string BaseUrl { get; private set; }

        public Mock<IReferenceDataApiConfiguration> ConfigurationMock { get; private set; }
        public IReferenceDataApiConfiguration Configuration => ConfigurationMock.Object;

        public Mock<SecureHttpClient> HttpClientMock { get; private set; }
        public SecureHttpClient HttpClient => HttpClientMock.Object;

        public ReferenceDataApiClientTestFixtures WithBaseUrl(string url)
        {
            ConfigurationMock
                .SetupGet(x => x.ApiBaseUrl)
                .Returns(url);

            BaseUrl = url;

            return this;
        }

        public List<string> CalledUrls { get; set; }
        public ReferenceDataApiClientTestFixtures WithCharityResponse(int charityno, Charity data)
        {
            HttpClientMock
                .Setup(c => c.GetAsync($"{BaseUrl}charities/{charityno}", true))
                .Callback<string, bool>((url, exceptionOnNotFound) => CalledUrls.Add(url))
                .ReturnsAsync(() => JsonConvert.SerializeObject(data));

            return this;
        }

        public ReferenceDataApiClientTestFixtures WithGetLatestDetailsResponse(OrganisationType organisationType, string registrationId, Organisation data, HttpStatusCode statusCode)
        {
            HttpClientMock
                .Setup(c => c.GetAsync($"{BaseUrl}get?identifier={registrationId}&type={organisationType}", It.IsAny<Func<HttpResponseMessage, bool>>()))
                .Callback<string, Func<HttpResponseMessage, bool>>((url, responseChecker) =>
                {
                    CalledUrls.Add(url);
                    responseChecker(new HttpResponseMessage(statusCode));
                })
                .ReturnsAsync(() => JsonConvert.SerializeObject(data));

            return this;
        }

        public ReferenceDataApiClientTestFixtures WithSearchPublicSectorResult(
            string searchTerm, 
            int pageNumber, 
            int pageSize,
            params PublicSectorOrganisation[] publicSectorOrganisations)
        {
            var data = new PagedApiResponse<PublicSectorOrganisation>
            {
                Data = new List<PublicSectorOrganisation>()
            };

            foreach (var publicSectorOrganisation in publicSectorOrganisations)
            {
                data.Data.Add(publicSectorOrganisation);
            }

            HttpClientMock
                .Setup(c => c.GetAsync(It.Is<string>(s => s == BuildSearchPublicSectorUrl(searchTerm, pageNumber, pageSize)), true))
                .Callback<string, bool>((url, exceptionOnNotFound) => CalledUrls.Add(url))
                .ReturnsAsync(() => JsonConvert.SerializeObject(data));

            return this;
        }

        public ReferenceDataApiClientTestFixtures WithSearchOrganisationsResult(
            string searchTerm,
            int maximumResults,
            params Organisation[] organisations)
        {
            var data = new List<Organisation>();
            data.AddRange(organisations);

            HttpClientMock
                .Setup(c => c.GetAsync(It.Is<string>(s => s == BuildSearchOrganisationUrl(searchTerm, maximumResults)), false))
                .Callback<string, bool>((url, exceptionOnNotFound) => CalledUrls.Add(url))
                .ReturnsAsync(() => JsonConvert.SerializeObject(data));

            return this;
        }

        public string BuildSearchOrganisationUrl(string searchTerm, int maximumResults)
        {
            return $"{BaseUrl}?searchTerm={HttpUtility.UrlPathEncode(searchTerm)}&maximumResults={maximumResults}";
        }

        public string BuildSearchPublicSectorUrl(string searchTerm, int pageNumber, int pageSize)
        {
            return $"{BaseUrl}publicsectorbodies?searchTerm={HttpUtility.UrlPathEncode(searchTerm)}&pageNumber={pageNumber}&pageSize={pageSize}";
        }

        public ReferenceDataApiClient CreateApiClient()
        {
            return new ReferenceDataApiClient(Configuration, HttpClient);
        }

        public ReferenceDataApiClientTestFixtures ChackUrlCalled(string url)
        {
            Assert.Contains(url, CalledUrls);
            return this;
        }

        public Task<Charity> GetCharityAsync(int charityNumber)
        {
            var apiClient = CreateApiClient();
            return apiClient.GetCharity(charityNumber);
        }

        public Task<Organisation> GetLatestDetailsAsync(OrganisationType organisationType, string registeredNumber)
        {
            var apiClient = CreateApiClient();
            return apiClient.GetLatestDetails(organisationType, registeredNumber);
        }

        public async Task CheckGetCharityAsync(int charityNumber, Action<Charity> checker)
        {
            var actualCharity = await GetCharityAsync(charityNumber);

            checker(actualCharity);
        }

        public Task<PagedApiResponse<PublicSectorOrganisation>> GetSearchPublicSectorAsync(string searchTerm)
        {
            var apiClient = CreateApiClient();

            return apiClient.SearchPublicSectorOrganisation(searchTerm, 0, 100);
        }

        public Task GetOrganisationSearch(string searchTerm, int maximumResults)
        {
            var apiClient = CreateApiClient();

            return apiClient.SearchOrganisations(searchTerm, maximumResults);
        }
    }
}

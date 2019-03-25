using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Configuration;
using SFA.DAS.ReferenceData.Domain.Http;
using SFA.DAS.ReferenceData.Domain.Models.Company;
using SFA.DAS.ReferenceData.Infrastructure.ExecutionPolicies;
using SFA.DAS.ReferenceData.Infrastructure.Services;

namespace SFA.DAS.ReferenceData.Infrastructure.UnitTests.Services.CompaniesHouseEmployerVerificationServiceTests
{
    internal class WhenIGetCompaniesHouseOrganisations
    {
        private const string SearchTerm = "TEST QUERY";
        private const string ComplexSearchTerm = "TEST & QUERY < &";
        private const int MaximumResults = 50;
      
        [Test]
        public void SearchOrganisations_WithValidParams_ShouldCallTheApiWithTheCorrectUrl()
        {
            var fixtures = new CompaniesHouseEmployerVerificationServiceTestFixtures()
                .WithBaseUrl(@"http://some-url/api/organisations/")
                .WithSearchOrganisationsResult(SearchTerm, MaximumResults);

            fixtures.GetOrganisationSearch(SearchTerm, MaximumResults);

            fixtures.CheckUrlCalled(fixtures.BuildSearchOrganisationUrl(SearchTerm, MaximumResults));
        }

        [Test]
        public void SearchOrganisations_WithValidComplexParams_ShouldCallTheApiWithTheCorrectUrl()
        {
            var fixtures = new CompaniesHouseEmployerVerificationServiceTestFixtures()
                .WithBaseUrl(@"http://some-url/api/organisations/")
                .WithSearchOrganisationsResult(ComplexSearchTerm, MaximumResults);

            fixtures.GetOrganisationSearch(ComplexSearchTerm, MaximumResults);

            fixtures.CheckUrlCalled(fixtures.BuildSearchOrganisationUrl(ComplexSearchTerm, MaximumResults));
        }
    }

    internal class CompaniesHouseEmployerVerificationServiceTestFixtures
    {
        private const string ApiKey = "APIKEY";

        public CompaniesHouseEmployerVerificationServiceTestFixtures()
        {
            Configuration = new ReferenceDataApiConfiguration();
            HttpClientMock = new Mock<IHttpClientWrapper>();
            LoggerMock = new Mock<ILog>();
            CompaniesExecutionPolicy = new CompaniesHouseExecutionPolicy(Logger);
            CalledUrls = new List<string>();
        }

        public string BaseUrl { get; private set; }

        public List<string> CalledUrls { get; set; }

        public ReferenceDataApiConfiguration Configuration;

        public Mock<IHttpClientWrapper> HttpClientMock { get; private set; }
        public IHttpClientWrapper HttpClient => HttpClientMock.Object;

        public Mock<ILog> LoggerMock { get; private set; }
        public ILog Logger => LoggerMock.Object;

        public CompaniesHouseExecutionPolicy CompaniesExecutionPolicy { get; private set; }

        public CompaniesHouseEmployerVerificationServiceTestFixtures WithBaseUrl(string url)
        {
            Configuration.CompaniesHouse = new CompaniesHouseConfiguration
            {
                ApiKey = ApiKey,
                BaseUrl = url
            };

            BaseUrl = url;

            return this;
        }

        public CompaniesHouseEmployerVerificationServiceTestFixtures WithSearchOrganisationsResult(string searchTerm,int maximumResults)
        {
            HttpClientMock
                .Setup(c => c.Get<CompanySearchResults>(It.IsAny<string>(), It.Is<string>(s => s.Equals(BuildSearchOrganisationUrl(searchTerm, maximumResults)))))
                .Callback<string, string>((api, url) => { CalledUrls.Add(url); })
                .ReturnsAsync(() => new CompanySearchResults());

            return this;
        }

        public string BuildSearchOrganisationUrl(string searchTerm, int maximumResults)
        {
            return $"{BaseUrl}/search/companies/?q={HttpUtility.UrlEncode(searchTerm)}&items_per_page={maximumResults}";
        }

        public CompaniesHouseEmployerVerificationService CreateApiClient()
        {
            return new CompaniesHouseEmployerVerificationService(Configuration, Logger, HttpClient, CompaniesExecutionPolicy);
        }

        public CompaniesHouseEmployerVerificationServiceTestFixtures CheckUrlCalled(string url)
        {
            Assert.Contains(url, CalledUrls);
            return this;
        }

        public Task GetOrganisationSearch(string searchTerm, int maximumResults)
        {
            var apiClient = CreateApiClient();
            return apiClient.FindCompany(searchTerm, maximumResults);
        }
    }
}

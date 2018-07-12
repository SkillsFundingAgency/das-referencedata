using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using SFA.DAS.ReferenceData.Api.Client.Dto;
using SFA.DAS.ReferenceData.Domain.Models.Charity;

namespace SFA.DAS.ReferenceData.Api.Client
{
    public class ReferenceDataApiClient : IReferenceDataApiClient
    {
        private readonly IReferenceDataApiConfiguration _configuration;
        private readonly SecureHttpClient _httpClient;

        public ReferenceDataApiClient(IReferenceDataApiConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new SecureHttpClient(configuration);
        }

        internal ReferenceDataApiClient(IReferenceDataApiConfiguration configuration, SecureHttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<Charity> GetCharity(int registrationNumber)
        {
            var baseUrl = GetBaseUrl();

            var url = $"{baseUrl}charities/{registrationNumber}";

            var json = await _httpClient.GetAsync(url);

            return JsonConvert.DeserializeObject<Charity>(json);
        }

        public async Task<PagedApiResponse<PublicSectorOrganisation>> SearchPublicSectorOrganisation(string searchTerm, int pageNumber, int pageSize)
        {
            var baseUrl = GetBaseUrl();

            var url = $"{baseUrl}publicsectorbodies?searchTerm={HttpUtility.UrlPathEncode(searchTerm)}&pageNumber={pageNumber}&pageSize={pageSize}";

            var json = await _httpClient.GetAsync(url);
            return JsonConvert.DeserializeObject<PagedApiResponse<PublicSectorOrganisation>>(json);

        }

        public async Task<IEnumerable<Organisation>> SearchOrganisations(string searchTerm, int maximumResults = 500)
        {
            var baseUrl = GetBaseUrl();

            var url = $"{baseUrl}?searchTerm={HttpUtility.UrlPathEncode(searchTerm)}&maximumResults={maximumResults}";
            
            var json = await _httpClient.GetAsync(url, false);

            if (json == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<IEnumerable<Organisation>>(json);
        }

        public async Task<PagedApiResponse<EducationOrganisation>> SearchEducationalOrganisation(string searchTerm, int pageNumber, int pageSize)
        {
            var baseUrl = GetBaseUrl();

            var url = $"{baseUrl}educational?searchTerm={HttpUtility.UrlPathEncode(searchTerm)}&pageNumber={pageNumber}&pageSize={pageSize}";

            var json = await _httpClient.GetAsync(url);

            return JsonConvert.DeserializeObject<PagedApiResponse<EducationOrganisation>>(json);
        }

        public async Task<Organisation> GetLatestDetails(OrganisationType organisationType, string identifier)
        {
            var baseUrl = GetBaseUrl();

            var url = $"{baseUrl}get?identifier={HttpUtility.UrlPathEncode(identifier)}&type={organisationType}";

            var json = await _httpClient.GetAsync(url);

            return JsonConvert.DeserializeObject<Organisation>(json);
        }

        private string GetBaseUrl()
        {
            var baseUrl = _configuration.ApiBaseUrl.EndsWith("/")
                ? _configuration.ApiBaseUrl
                : _configuration.ApiBaseUrl + "/";
            return baseUrl;
        }
    }
}

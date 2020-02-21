using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using SFA.DAS.ReferenceData.Api.Client.Dto;
using SFA.DAS.ReferenceData.Types.DTO;
using SFA.DAS.ReferenceData.Types.Exceptions;

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
            var url = $"{baseUrl}publicsectorbodies?searchTerm={HttpUtility.UrlEncode(searchTerm)}&pageNumber={pageNumber}&pageSize={pageSize}";

            var json = await _httpClient.GetAsync(url);
            return JsonConvert.DeserializeObject<PagedApiResponse<PublicSectorOrganisation>>(json);

        }

        public async Task<IEnumerable<Organisation>> SearchOrganisations(string searchTerm, int maximumResults = 500)
        {
            var baseUrl = GetBaseUrl();
            var url = $"{baseUrl}?searchTerm={HttpUtility.UrlEncode(searchTerm)}&maximumResults={maximumResults}";
            
            var json = await _httpClient.GetAsync(url, false);
            return json == null ? null : JsonConvert.DeserializeObject<IEnumerable<Organisation>>(json);
        }

        public async Task<PagedApiResponse<EducationOrganisation>> SearchEducationalOrganisation(string searchTerm, int pageNumber, int pageSize)
        {
            var baseUrl = GetBaseUrl();
            var url = $"{baseUrl}educational?searchTerm={HttpUtility.UrlEncode(searchTerm)}&pageNumber={pageNumber}&pageSize={pageSize}";

            var json = await _httpClient.GetAsync(url);
            return JsonConvert.DeserializeObject<PagedApiResponse<EducationOrganisation>>(json);
        }

        public async Task<Organisation> GetLatestDetails(OrganisationType organisationType, string identifier)
        {
            var baseUrl = GetBaseUrl();
            var url = $"{baseUrl}get?identifier={HttpUtility.UrlEncode(identifier)}&organisationType={organisationType}";

            var json = await _httpClient.GetAsync(url, response =>
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound: throw new OrganisationNotFoundException(response.ReasonPhrase);
                    case HttpStatusCode.BadRequest: throw new InvalidGetOrganisationRequest(response.ReasonPhrase);
                }
                return true;
            });

            return JsonConvert.DeserializeObject<Organisation>(json);
        }

        public async Task<OrganisationType[]> GetIdentifiableOrganisationTypes()
        {
            var baseUrl = GetBaseUrl();
            var url = $"{baseUrl}IdentifiableOrganisationTypes";

            var json = await _httpClient.GetAsync(url, null);
            return JsonConvert.DeserializeObject<OrganisationType[]>(json);
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

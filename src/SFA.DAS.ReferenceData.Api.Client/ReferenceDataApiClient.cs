using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SFA.DAS.ReferenceData.Api.Client
{
    public class ReferenceDataApiClient :IReferenceDataApiClient
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

        public async Task<Dto.Charity> GetCharity(int registrationNumber)
        {
            var baseUrl = _configuration.ApiBaseUrl.EndsWith("/")
                ? _configuration.ApiBaseUrl
                : _configuration.ApiBaseUrl + "/";

            var url = $"{baseUrl}charities/{registrationNumber}";

            var json = await _httpClient.GetAsync(url);

            return JsonConvert.DeserializeObject<Dto.Charity>(json);
        }

        public async Task<Dto.PagedApiResponse<Dto.PublicSectorOrganisation>> SearchPublicSectorOrganisation(string searchTerm, int pageNumber, int pageSize)
        {
            var baseUrl = _configuration.ApiBaseUrl.EndsWith("/")
                ? _configuration.ApiBaseUrl
                : _configuration.ApiBaseUrl + "/";

            var url = $"{baseUrl}publicsectorbodies?searchTerm={searchTerm}&pageNumber={pageNumber}&pageSize={pageSize}";

            var json = await _httpClient.GetAsync(url);
            return JsonConvert.DeserializeObject<Dto.PagedApiResponse<Dto.PublicSectorOrganisation>>(json);

        }
    }
}

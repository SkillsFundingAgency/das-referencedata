using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SFA.DAS.ReferenceData.Api.Client
{
    public class ReferenceDataApiClient :IReferenceDataApiClient
    {
        private readonly ReferenceDataApiConfiguration _configuration;
        private readonly SecureHttpClient _httpClient;

        public ReferenceDataApiClient(ReferenceDataApiConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new SecureHttpClient(configuration);
        }

        internal ReferenceDataApiClient(ReferenceDataApiConfiguration configuration, SecureHttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<Dto.Charity> GetCharity(int regisrationNumber)
        {
            var baseUrl = _configuration.ApiBaseUrl.EndsWith("/")
                ? _configuration.ApiBaseUrl
                : _configuration.ApiBaseUrl + "/";

            var url = $"{baseUrl}charity/{regisrationNumber}";

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

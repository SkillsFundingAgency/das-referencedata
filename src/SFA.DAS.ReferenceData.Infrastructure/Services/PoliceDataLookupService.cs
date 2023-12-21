using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Configuration;
using SFA.DAS.ReferenceData.Domain.Http;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models;
using SFA.DAS.ReferenceData.Infrastructure.Responses;
using SFA.DAS.ReferenceData.Types.DTO;

namespace SFA.DAS.ReferenceData.Infrastructure.Services
{
    public class PoliceDataLookupService : IPoliceDataLookupService
    {
        private readonly IHttpClientWrapper _httpClientWrapper;

        public PoliceDataLookupService(
            IHttpClientWrapper httpClientWrapper,
            ReferenceDataApiConfiguration configuration)
        {
            _httpClientWrapper = httpClientWrapper;
            
            _httpClientWrapper.BaseUrl = configuration.PoliceForcesUrl;
        }

        public async Task<PublicSectorOrganisationLookUp> GetGbPoliceForces()
        {
            var response = new PublicSectorOrganisationLookUp();
            var policeApiResponse = await _httpClientWrapper.Get<List<PoliceApiResponse>>(string.Empty, string.Empty);

            if (policeApiResponse != null)
            {
                var publicSectorOrgs = policeApiResponse.Select(r =>
                    new PublicSectorOrganisation
                    {
                        Name = r.Name,
                        Source = DataSource.Police
                    }).ToList();

                response.Organisations = publicSectorOrgs;
            }

            return response;
        }
    }
}
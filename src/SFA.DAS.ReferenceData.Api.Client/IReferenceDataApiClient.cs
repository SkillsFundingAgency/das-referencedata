using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Api.Client.Dto;

namespace SFA.DAS.ReferenceData.Api.Client
{
    public interface IReferenceDataApiClient
    {
        Task<Charity> GetCharity(int registrationNumber);

        Task<PagedApiResponse<PublicSectorOrganisation>> SearchPublicSectorOrganisation(string searchTerm, int pageNumber, int pageSize);

        Task<IEnumerable<Organisation>> SearchOrganisations(string searchTerm, int maximumResults = 500);

        Task<PagedApiResponse<EducationOrganisation>> SearchEducationalOrganisation(string searchTerm, int pageNumber, int pageSize);
    }
}

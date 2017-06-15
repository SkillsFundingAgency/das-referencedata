using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Api.Client.Dto;

namespace SFA.DAS.ReferenceData.Api.Client
{
    public interface IReferenceDataApiClient
    {
        Task<Charity> GetCharity(int registrationNumber);

        Task<PagedApiResponse<PublicSectorOrganisation>> SearchPublicSectorOrganisation(string searchTerm, int pageNumber, int pageSize);

        Task<PagedApiResponse<Organisation>> SearchOrganisations(string searchTerm, int pageNumber = 1, int pageSize = 20, int maximumResults = 500);

        Task<PagedApiResponse<EducationOrganisation>> SearchEducationalOrganisation(string searchTerm, int pageNumber, int pageSize);
    }
}

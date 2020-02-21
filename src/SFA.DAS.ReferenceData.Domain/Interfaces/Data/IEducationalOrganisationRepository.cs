using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Models.Data;
using SFA.DAS.ReferenceData.Types;
using SFA.DAS.ReferenceData.Types.DTO;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Data
{
    public interface IEducationalOrganisationRepository
    {
        Task<PagedResult<EducationOrganisation>> FindOrganisations(string searchTerm, int pageSize, int pageNumber);
        Task<EducationOrganisation> FindOrganisationByUrn(int urn);
    }
}

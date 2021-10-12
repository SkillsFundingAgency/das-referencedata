using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Interfaces.Caching;
using SFA.DAS.ReferenceData.Domain.Models.Data;
using SFA.DAS.ReferenceData.Types;
using SFA.DAS.ReferenceData.Types.DTO;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Data
{
    public interface IPublicSectorOrganisationRepository : ICachedRepository
    {
        Task<PagedResult<PublicSectorOrganisation>> FindOrganisations(string searchTerm, int pageSize, int pageNumber);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Models;
using SFA.DAS.ReferenceData.Domain.Models.Data;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Data
{
    public interface IPublicSectorOrganisationRepository
    {
        Task<PagedResult<PublicSectorOrganisation>> FindOrganisations(string searchTerm, int pageSize, int pageNumber);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Data
{
    public interface IOrganisationRepository
    {
        Task<IEnumerable<PublicSectorOrganisation>> GetPublicSectorOrganisations();
    }
}

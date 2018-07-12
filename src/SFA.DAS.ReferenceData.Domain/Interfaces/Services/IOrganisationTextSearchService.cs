using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Types;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Services
{
    public interface IOrganisationTextSearchService
    {
        Task<IEnumerable<Organisation>> Search(string searchTerm, int maximumRecords);
    }
}

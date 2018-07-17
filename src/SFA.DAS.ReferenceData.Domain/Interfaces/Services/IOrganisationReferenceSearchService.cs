using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Types;
using SFA.DAS.ReferenceData.Types.DTO;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Services
{
    public interface IOrganisationReferenceSearchService
    {
        OrganisationType OrganisationType { get; }
        bool IsSearchTermAReference(string searchTerm);
        Task<Organisation> Search(string reference);
    }
}

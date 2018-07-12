using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Types;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Services
{
    public interface IOrganisationReferenceSearchService
    {
        OrganisationType OrganisationType { get; }
        bool IsSearchTermAReference(string searchTerm);
        Task<Organisation> Search(string reference);
    }
}

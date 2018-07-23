using SFA.DAS.Common.Domain.Types;
using SFA.DAS.ReferenceData.Types.DTO;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Services
{
    public interface IOrganisationTypeHelper
    {
        bool TryGetReferenceSearcher(OrganisationType organisationType, out IOrganisationReferenceSearchService referenceSearcher);
    }
}
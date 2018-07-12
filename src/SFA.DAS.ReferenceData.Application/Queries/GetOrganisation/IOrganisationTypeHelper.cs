using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Types;

namespace SFA.DAS.ReferenceData.Application.Queries.GetOrganisation
{
    public interface IOrganisationTypeHelper
    {
        bool TryGetReferenceSearcher(OrganisationType organisationType, out IOrganisationReferenceSearchService referenceSearcher);
    }
}
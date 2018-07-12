using System.Collections.Generic;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Types;

namespace SFA.DAS.ReferenceData.Application.Queries.GetOrganisation
{
    public class OrganisationTypeHelper : IOrganisationTypeHelper
    {
        private readonly Dictionary<OrganisationType, IOrganisationReferenceSearchService> _referenceSearchers = new Dictionary<OrganisationType, IOrganisationReferenceSearchService>();

        public OrganisationTypeHelper(IEnumerable<IOrganisationReferenceSearchService> referenceSearchServices)
        {
            _referenceSearchers = new Dictionary<OrganisationType, IOrganisationReferenceSearchService>();
            foreach (var referenceSearchService in referenceSearchServices)
            {
                _referenceSearchers.Add(referenceSearchService.OrganisationType, referenceSearchService);
            }
        }

        public bool TryGetReferenceSearcher(OrganisationType organisationType, out IOrganisationReferenceSearchService referenceSearcher)
        {
            return _referenceSearchers.TryGetValue(organisationType, out referenceSearcher);
        }
    }
}
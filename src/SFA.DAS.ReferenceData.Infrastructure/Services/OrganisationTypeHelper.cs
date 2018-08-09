using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Types.DTO;
using SFA.DAS.ReferenceData.Types.Exceptions;

namespace SFA.DAS.ReferenceData.Infrastructure.Services
{
    public class OrganisationTypeHelper : IOrganisationTypeHelper
    {
        private readonly Dictionary<OrganisationType, IOrganisationReferenceSearchService> _referenceSearchers = new Dictionary<OrganisationType, IOrganisationReferenceSearchService>();

        public OrganisationTypeHelper(IEnumerable<IOrganisationReferenceSearchService> referenceSearchServices)
        {
            _referenceSearchers = new Dictionary<OrganisationType, IOrganisationReferenceSearchService>();
            foreach (var referenceSearchService in referenceSearchServices)
            {
                if (_referenceSearchers.ContainsKey(referenceSearchService.OrganisationType))
                {
                    throw new ReferenceDataException($"The organisation type {referenceSearchService.OrganisationType} has more than one implementation of {nameof(IOrganisationReferenceSearchService)} supporting it");
                }

                _referenceSearchers.Add(referenceSearchService.OrganisationType, referenceSearchService);
            }
        }

        public bool TryGetReferenceSearcher(OrganisationType organisationType, out IOrganisationReferenceSearchService referenceSearcher)
        {
            return _referenceSearchers.TryGetValue(organisationType, out referenceSearcher);
        }

        public OrganisationType[] GetIdentifiableOrganisationTypes()
        {
            return _referenceSearchers.Select(rs => rs.Value.OrganisationType).ToArray();
        }
    }
}
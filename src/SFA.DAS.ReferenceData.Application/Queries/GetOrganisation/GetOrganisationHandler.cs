using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ReferenceData.Api.Client;
using SFA.DAS.ReferenceData.Api.Client.Dto;
using SFA.DAS.ReferenceData.Api.Client.Exceptions;
using SFA.DAS.ReferenceData.Application.Queries.SearchOrganisations;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models.Organisation;

namespace SFA.DAS.ReferenceData.Application.Queries.GetOrganisation
{
    public interface IOrganisationTypeHelper
    {
        bool TryGetReferenceSearcher(OrganisationType organisationType, out IOrganisationReferenceSearchService referenceSearcher);
    }

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

    public class GetOrganisationHandler : IAsyncRequestHandler<GetOrganisationQuery, GetOrganisationResponse>
    {
        private readonly IOrganisationTypeHelper _organisationTypeHelper;

        public GetOrganisationHandler(IOrganisationTypeHelper organisationTypeHelper)
        {
            _organisationTypeHelper = organisationTypeHelper;
        }

        public async Task<GetOrganisationResponse> Handle(GetOrganisationQuery query)
        {
            if (!_organisationTypeHelper.TryGetReferenceSearcher(query.OrganisationType,
                out IOrganisationReferenceSearchService referenceSearcher))
            {
                throw new OperationNotSupportedForOrganisationType(query.OrganisationType, "Find by id");
            }

            if (referenceSearcher.IsSearchTermAReference(query.Identifier))
            {

            }

            return await Task.FromResult(new GetOrganisationResponse());
        }
    }
}

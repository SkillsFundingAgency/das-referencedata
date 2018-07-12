using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Types.Exceptions;

namespace SFA.DAS.ReferenceData.Application.Queries.GetOrganisation
{
    public class GetOrganisationHandler : IAsyncRequestHandler<GetOrganisationQuery, GetOrganisationResponse>
    {
        private readonly IOrganisationTypeHelper _organisationTypeHelper;

        public GetOrganisationHandler(IOrganisationTypeHelper organisationTypeHelper)
        {
            _organisationTypeHelper = organisationTypeHelper;
        }

        public async Task<GetOrganisationResponse> Handle(GetOrganisationQuery query)
        {
            if (!_organisationTypeHelper.TryGetReferenceSearcher(query.OrganisationType, out var referenceSearcher))
            {
                throw new OperationNotSupportedForOrganisationType(query.OrganisationType, "Find by id");
            }

            if (referenceSearcher.IsSearchTermAReference(query.Identifier))
            {
                throw new ReferenceDataException($"The supplied identifier is not in a format recognised by the reference handler for organisation type ({query.OrganisationType}). Handler type is {referenceSearcher.GetType().FullName}");
            }

            return new GetOrganisationResponse
            {
                Organisation = await referenceSearcher.Search(query.Identifier)
            };
        }
    }
}

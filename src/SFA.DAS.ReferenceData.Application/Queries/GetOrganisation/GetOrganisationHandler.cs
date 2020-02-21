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
                throw new OperationNotSupportedForOrganisationTypeException(query.OrganisationType, "Find by id");
            }

            if (!referenceSearcher.IsSearchTermAReference(query.Identifier))
            {
                throw new BadOrganisationIdentifierException(query.OrganisationType, query.Identifier);
            }

            var organisation = await referenceSearcher.Search(query.Identifier);

            if (organisation == null)
            {
                throw new OrganisationNotFoundException(query.OrganisationType, query.Identifier);
            }

            return new GetOrganisationResponse
            {
                Organisation = organisation
            };
        }
    }
}

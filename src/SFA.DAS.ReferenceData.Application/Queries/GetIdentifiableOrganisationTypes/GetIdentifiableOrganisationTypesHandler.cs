using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;

namespace SFA.DAS.ReferenceData.Application.Queries.GetIdentifiableOrganisationTypes
{
    public class GetIdentifiableOrganisationTypesHandler : IAsyncRequestHandler<GetIdentifiableOrganisationTypesQuery, GetIdentifiableOrganisationTypesResponse>
    {
        private readonly IOrganisationTypeHelper _organisationTypeHelper;

        public GetIdentifiableOrganisationTypesHandler(IOrganisationTypeHelper organisationTypeHelper)
        {
            _organisationTypeHelper = organisationTypeHelper;
        }

        public Task<GetIdentifiableOrganisationTypesResponse> Handle(GetIdentifiableOrganisationTypesQuery query)
        {
            return Task.FromResult(new GetIdentifiableOrganisationTypesResponse
            {
                OrganisationTypes = _organisationTypeHelper.GetIdentifiableOrganisationTypes()
            });
        }
    }
}

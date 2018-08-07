using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;

namespace SFA.DAS.ReferenceData.Application.Queries.GetLocateableOrganisationTypes
{
    public class GetLocateableOrganisationTypesHandler : IAsyncRequestHandler<GetLocateableOrganisationTypesQuery, GetLocateableOrganisationTypesResponse>
    {
        private readonly IOrganisationTypeHelper _organisationTypeHelper;

        public GetLocateableOrganisationTypesHandler(IOrganisationTypeHelper organisationTypeHelper)
        {
            _organisationTypeHelper = organisationTypeHelper;
        }

        public Task<GetLocateableOrganisationTypesResponse> Handle(GetLocateableOrganisationTypesQuery query)
        {
            return Task.FromResult(new GetLocateableOrganisationTypesResponse
            {
                OrganisationTypes = _organisationTypeHelper.GetLocateableOrganisationTypes()
            });
        }
    }
}

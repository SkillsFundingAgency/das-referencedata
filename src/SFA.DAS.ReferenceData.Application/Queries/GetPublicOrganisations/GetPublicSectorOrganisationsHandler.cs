using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;

namespace SFA.DAS.ReferenceData.Application.Queries.GetPublicOrganisations
{
    public class GetPublicSectorOrganisationsHandler : IAsyncRequestHandler<GetPublicSectorOrgainsationsQuery, GetPublicSectorOrganisationsResponse>
    {
        private readonly IPublicSectorOrganisationLookUpService _lookupService;

        public GetPublicSectorOrganisationsHandler(IPublicSectorOrganisationLookUpService lookupService)
        {
            _lookupService = lookupService;
        }

        public async Task<GetPublicSectorOrganisationsResponse> Handle(GetPublicSectorOrgainsationsQuery query)
        {
            return new GetPublicSectorOrganisationsResponse
            {
                Organisations = await _lookupService.GetOrganisations()
            };
        }
    }
}

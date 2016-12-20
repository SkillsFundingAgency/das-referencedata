using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;

namespace SFA.DAS.ReferenceData.Application.Queries.GetPublicOrganisations
{
    public class GetPublicSectorOrganisationsHandler : IAsyncRequestHandler<GetPublicSectorOrgainsationsQuery, GetPublicSectorOrganisationsResponse>
    {
        private readonly IPubicSectorOrganisationRepository _pubicSectorOrganisationRepository;


        public GetPublicSectorOrganisationsHandler(IPubicSectorOrganisationRepository pubicSectorOrganisationRepository)
        {
            _pubicSectorOrganisationRepository = pubicSectorOrganisationRepository;
        }

        public async Task<GetPublicSectorOrganisationsResponse> Handle(GetPublicSectorOrgainsationsQuery query)
        {
            return new GetPublicSectorOrganisationsResponse
            {
                Organisations = await _pubicSectorOrganisationRepository.GetOrganisations()
            };
        }
    }
}

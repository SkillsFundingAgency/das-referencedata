using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;

namespace SFA.DAS.ReferenceData.Application.Queries.GetPublicOrganisations
{
    public class GetPublicSectorOrganisationsHandler : IAsyncRequestHandler<GetPublicSectorOrgainsationsQuery, GetPublicSectorOrganisationsResponse>
    {
        private readonly IPublicSectorOrganisationRepository _publicSectorOrganisationRepository;


        public GetPublicSectorOrganisationsHandler(IPublicSectorOrganisationRepository publicSectorOrganisationRepository)
        {
            _publicSectorOrganisationRepository = publicSectorOrganisationRepository;
        }

        public async Task<GetPublicSectorOrganisationsResponse> Handle(GetPublicSectorOrgainsationsQuery query)
        {
            return new GetPublicSectorOrganisationsResponse
            {
                Organisations = await _publicSectorOrganisationRepository.GetOrganisations()
            };
        }
    }
}

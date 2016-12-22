using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;

namespace SFA.DAS.ReferenceData.Application.Queries.GetPublicOrganisations
{
    public class GetPublicSectorOrganisationsHandler : IAsyncRequestHandler<FindPublicSectorOrgainsationQuery, FindPublicSectorOrganisationResponse>
    {
        private readonly IPublicSectorOrganisationRepository _publicSectorOrganisationRepository;


        public GetPublicSectorOrganisationsHandler(IPublicSectorOrganisationRepository publicSectorOrganisationRepository)
        {
            _publicSectorOrganisationRepository = publicSectorOrganisationRepository;
        }

        public async Task<FindPublicSectorOrganisationResponse> Handle(FindPublicSectorOrgainsationQuery query)
        {
            return new FindPublicSectorOrganisationResponse
            {
                Organisations = await _publicSectorOrganisationRepository.FindOrganisations(
                    query.SearchTerm, 
                    query.PageSize,
                    query.PageNumber)
            };
        }
    }
}

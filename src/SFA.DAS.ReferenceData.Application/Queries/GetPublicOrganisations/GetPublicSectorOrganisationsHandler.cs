using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ReferenceData.Application.Queries.GetPublicOrganisations
{
    public class GetPublicSectorOrganisationsHandler : IAsyncRequestHandler<GetPublicSectorOrgainsationsQuery, GetPublicSectorOrganisationsResponse>
    {
        public Task<GetPublicSectorOrganisationsResponse> Handle(GetPublicSectorOrgainsationsQuery query)
        {
            throw new System.NotImplementedException();
        }
    }
}

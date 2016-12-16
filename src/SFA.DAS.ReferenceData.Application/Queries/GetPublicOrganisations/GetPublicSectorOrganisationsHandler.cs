using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;

namespace SFA.DAS.ReferenceData.Application.Queries.GetPublicOrganisations
{
    public class GetPublicSectorOrganisationsHandler : IAsyncRequestHandler<GetPublicSectorOrgainsationsQuery, GetPublicSectorOrganisationsResponse>
    {
        private readonly IOrganisationRepository _repositoryObject;

        public GetPublicSectorOrganisationsHandler(IOrganisationRepository repositoryObject)
        {
            _repositoryObject = repositoryObject;
        }

        public async Task<GetPublicSectorOrganisationsResponse> Handle(GetPublicSectorOrgainsationsQuery query)
        {
            var organisations = await _repositoryObject.GetPublicSectorOrganisations();

            return new GetPublicSectorOrganisationsResponse
            {
                Organisations = organisations
            };
        }
    }
}

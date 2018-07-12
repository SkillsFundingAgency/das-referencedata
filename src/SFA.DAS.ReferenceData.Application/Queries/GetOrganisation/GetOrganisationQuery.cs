using MediatR;
using SFA.DAS.ReferenceData.Api.Client.Dto;
using SFA.DAS.ReferenceData.Types;

namespace SFA.DAS.ReferenceData.Application.Queries.GetOrganisation
{
    public class GetOrganisationQuery : IAsyncRequest<GetOrganisationResponse>
    {
        public string Identifier { get; set; }
        public OrganisationType OrganisationType { get; set; }
    }
}

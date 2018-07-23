using MediatR;
using SFA.DAS.Common.Domain.Types;
using SFA.DAS.ReferenceData.Types.DTO;

namespace SFA.DAS.ReferenceData.Application.Queries.GetOrganisation
{
    public class GetOrganisationQuery : IAsyncRequest<GetOrganisationResponse>
    {
        public string Identifier { get; set; }
        public OrganisationType OrganisationType { get; set; }
    }
}

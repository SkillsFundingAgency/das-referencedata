using MediatR;
using SFA.DAS.ReferenceData.Domain.Models.Organisation;
using SFA.DAS.ReferenceData.Api.Client.Dto;

namespace SFA.DAS.ReferenceData.Application.Queries.GetOrganisation
{
    public class GetOrganisationQuery : IAsyncRequest<GetOrganisationResponse>
    {
        public string Identifier { get; set; }
        public OrganisationType OrganisationType { get; set; }
    }
}

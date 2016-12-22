using MediatR;

namespace SFA.DAS.ReferenceData.Application.Queries.GetPublicOrganisations
{
    public class FindPublicSectorOrgainsationQuery : IAsyncRequest<FindPublicSectorOrganisationResponse>
    {
        public string SearchTerm { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}

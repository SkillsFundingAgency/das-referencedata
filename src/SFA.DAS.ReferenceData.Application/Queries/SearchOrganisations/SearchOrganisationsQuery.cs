using MediatR;

namespace SFA.DAS.ReferenceData.Application.Queries.SearchOrganisations
{
    public class SearchOrganisationsQuery : IAsyncRequest<SearchOrganisationsResponse>
    {
        public string SearchTerm { get; set; }
        public int MaximumResults { get; set; }
    }
}

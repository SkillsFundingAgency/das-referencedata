using MediatR;

namespace SFA.DAS.ReferenceData.Application.Queries.GetEducationalOrganisations
{
    public class FindEducationalOrganisationsQuery : IAsyncRequest<FindEducationalOrganisationsResponse>
    {
        public string SearchTerm { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}

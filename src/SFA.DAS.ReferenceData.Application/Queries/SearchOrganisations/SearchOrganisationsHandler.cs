using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models.Organisation;

namespace SFA.DAS.ReferenceData.Application.Queries.SearchOrganisations
{
    public class SearchOrganisationsHandler : IAsyncRequestHandler<SearchOrganisationsQuery, SearchOrganisationsResponse>
    {
        private readonly IEnumerable<IOrganisationTextSearchService> _textSearchServices;

        public SearchOrganisationsHandler(IEnumerable<IOrganisationTextSearchService> textSearchServices)
        {
            _textSearchServices = textSearchServices;
        }

        public async Task<SearchOrganisationsResponse> Handle(SearchOrganisationsQuery query)
        {
            var results = await PerformTextSearch(query);

            return new SearchOrganisationsResponse { Organisations = CombineSearchResults(results) };
        }

        private static IEnumerable<Organisation> CombineSearchResults(IEnumerable<Organisation>[] results)
        {
            return results.SelectMany(x => x.Select(y => y));
        }

        private async Task<IEnumerable<Organisation>[]> PerformTextSearch(SearchOrganisationsQuery query)
        {
            var tasks = _textSearchServices.Select(x => x.Search(query.SearchTerm, query.MaximumResults)).ToList();
            var results = await Task.WhenAll(tasks);
            return results;
        }
    }
}

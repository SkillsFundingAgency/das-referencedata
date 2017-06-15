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

            var combinedResults = CombineSearchResults(results);
            return new SearchOrganisationsResponse { Organisations = LimitResultsToMaximum(combinedResults, query.MaximumResults) };
        }

        private IEnumerable<Organisation> LimitResultsToMaximum(IEnumerable<Organisation> combinedResults, int maximumResults)
        {
            return combinedResults.Take(maximumResults);
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

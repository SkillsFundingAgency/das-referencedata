using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ReferenceData.Api.Client.Dto;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Types;

namespace SFA.DAS.ReferenceData.Application.Queries.SearchOrganisations
{
    public class SearchOrganisationsHandler : IAsyncRequestHandler<SearchOrganisationsQuery, SearchOrganisationsResponse>
    {
        private readonly IEnumerable<IOrganisationTextSearchService> _textSearchServices;
        private readonly IEnumerable<IOrganisationReferenceSearchService> _referenceSearchServices;

        public SearchOrganisationsHandler(IEnumerable<IOrganisationTextSearchService> textSearchServices, IEnumerable<IOrganisationReferenceSearchService> referenceSearchServices)
        {
            _textSearchServices = textSearchServices;
            _referenceSearchServices = referenceSearchServices;
        }

        public async Task<SearchOrganisationsResponse> Handle(SearchOrganisationsQuery query)
        {
            var matchingReferenceSearches = _referenceSearchServices.Where(x => x.IsSearchTermAReference(query.SearchTerm)).ToArray();

            IEnumerable<Organisation> results;

            if (matchingReferenceSearches.Any())
            {
                results = await SearchByReference(query.SearchTerm, matchingReferenceSearches);
            }
            else
            {
                results = await SearchByText(query);
            }

            return new SearchOrganisationsResponse { Organisations = results };
        }

        private static async Task<IEnumerable<Organisation>> SearchByReference(string reference, IEnumerable<IOrganisationReferenceSearchService> matchingReferenceSearches)
        {
            return await PerformReferenceSearch(reference, matchingReferenceSearches);
        }

        private async Task<IEnumerable<Organisation>> SearchByText(SearchOrganisationsQuery query)
        {
            var results = await PerformTextSearch(query);

            var combinedResults = CombineSearchResults(results);
            return LimitResultsToMaximum(combinedResults, query.MaximumResults);
        }

        private static IEnumerable<Organisation> LimitResultsToMaximum(IEnumerable<Organisation> combinedResults, int maximumResults)
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

        private static async Task<IEnumerable<Organisation>> PerformReferenceSearch(string reference, IEnumerable<IOrganisationReferenceSearchService> matchingReferenceSearches)
        {
            var tasks = matchingReferenceSearches.Select(x => x.Search(reference)).ToList();
            var results = await Task.WhenAll(tasks);
            return results.Where(x => x != null);
        }
    }
}

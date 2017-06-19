using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.ReferenceData.Domain.Models.Company
{
    public class CompanySearchResults
    {
        [JsonProperty("items")]
        public IEnumerable<CompanySearchResultsItem> Companies { get; set; }
    }
}

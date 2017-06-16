using System;
using Newtonsoft.Json;

namespace SFA.DAS.ReferenceData.Domain.Models.Company
{
    public class CompanySearchResultsItem
    {
        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("company_number")]
        public string CompanyNumber { get; set; }

        [JsonProperty("date_of_creation")]
        public DateTime? DateOfIncorporation { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("company_status")]
        public string CompanyStatus { get; set; }
    }
}

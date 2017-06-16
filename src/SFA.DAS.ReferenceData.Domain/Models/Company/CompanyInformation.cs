using System;
using Newtonsoft.Json;

namespace SFA.DAS.ReferenceData.Domain.Models.Company
{
    public class CompanyInformation
    {
        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("company_number")]
        public string CompanyNumber { get; set; }

        [JsonProperty("date_of_creation")]
        public DateTime? DateOfIncorporation { get; set; }

        [JsonProperty("registered_office_address")]
        public Address RegisteredAddress { get; set; }

        [JsonProperty("company_status")]
        public string CompanyStatus { get; set; }
    }
}
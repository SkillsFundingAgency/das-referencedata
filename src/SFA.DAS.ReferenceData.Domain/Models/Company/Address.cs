using Newtonsoft.Json;

namespace SFA.DAS.ReferenceData.Domain.Models.Company
{
    public class Address
    {
        [JsonProperty("premises")]
        public string Premises { get; set; }
        [JsonProperty("address_line_1")]
        public string CompaniesHouseLine1 { get; set; }

        public string Line1 => string.Join(" ", Premises, CompaniesHouseLine1);

        [JsonProperty("address_line_2")]
        public string Line2 { get; set; }
        [JsonProperty("locality")]
        public string TownOrCity { get; set; }
        [JsonProperty("region")]
        public string County { get; set; }
        [JsonProperty("postal_code")]
        public string PostCode { get; set; }
    }
}
namespace SFA.DAS.ReferenceData.Api.Client.TestHarness
{
    public class ReferenceDataApiConfiguration : IReferenceDataApiConfiguration
    {
        public string ApiBaseUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string IdentifierUri { get; set; }
        public string Tenant { get; set; }
    }
}

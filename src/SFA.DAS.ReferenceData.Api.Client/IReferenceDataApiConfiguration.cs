namespace SFA.DAS.ReferenceData.Api.Client
{
    public interface IReferenceDataApiConfiguration
    {
        string ApiBaseUrl { get; set; }
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string IdentifierUri { get; set; }
        string Tenant { get; set; }
    }
}

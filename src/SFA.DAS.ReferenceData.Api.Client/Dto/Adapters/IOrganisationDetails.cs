namespace SFA.DAS.ReferenceData.Api.Client.Dto.Adapters
{
    public interface IOrganisation
    {
        OrganisationType OrganisationType { get; }
        string  Identifier { get; }
        string Name { get; }
        string AddressLine1 { get; }
        string AddressLine2 { get; }
        string AddressLine3 { get; }
        string AddressLine4 { get; }
        string AddressLine5 { get; }
        string PostCode { get; }
    }
}
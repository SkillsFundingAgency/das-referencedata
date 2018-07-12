namespace SFA.DAS.ReferenceData.Api.Client.Dto.Adapters
{
    public class OrganisationToNameAndAddressAdapter : IOrganisationDetails
    {
        private readonly Organisation _organisation;

        public OrganisationToNameAndAddressAdapter(Organisation organisation)
        {
            _organisation = organisation;
        }

        public OrganisationType OrganisationType => OrganisationType.Company;
        public string Identifier => _organisation.Code;
        public string Name => _organisation.Name;
        public string AddressLine1 => _organisation.Address.Line1;
        public string AddressLine2 => _organisation.Address.Line2;
        public string AddressLine3 => _organisation.Address.Line3;
        public string AddressLine4 => _organisation.Address.Line4;
        public string AddressLine5 => _organisation.Address.Line5;
        public string PostCode => _organisation.Address.Postcode;
    }
}
using System.Globalization;

namespace SFA.DAS.ReferenceData.Api.Client.Dto.Adapters
{
    public class CharityToNameAndAddressAdapter : IOrganisationDetails
    {
        private readonly Charity _charity;

        public CharityToNameAndAddressAdapter(Charity charity)
        {
            _charity = charity;
        }

        public OrganisationType OrganisationType => OrganisationType.Charity;
        public string Identifier => _charity.RegistrationNumber.ToString(CultureInfo.InvariantCulture);
        public string Name => _charity.Name;
        public string AddressLine1 => _charity.Address1;
        public string AddressLine2 => _charity.Address2;
        public string AddressLine3 => _charity.Address3;
        public string AddressLine4 => _charity.Address4;
        public string AddressLine5 => _charity.Address5;
        public string PostCode => _charity.PostCode;
    }
}
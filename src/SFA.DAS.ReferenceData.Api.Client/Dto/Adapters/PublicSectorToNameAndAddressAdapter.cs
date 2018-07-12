using SFA.DAS.ReferenceData.Api.Client.Dto.Adapters;

namespace SFA.DAS.ReferenceData.Api.Client.Dto
{
    public class PublicSectorToOrganisationDetailsAdapter : IOrganisationDetails
    {
        private readonly PublicSectorOrganisation _publicSectorOrganisation;

        public PublicSectorToOrganisationDetailsAdapter(PublicSectorOrganisation publicSectorOrganisation)
        {
            _publicSectorOrganisation = publicSectorOrganisation;
        }
        public OrganisationType OrganisationType => OrganisationType.PublicSector;
        public string Identifier => _publicSectorOrganisation.OrganisationCode;
        public string Name => _publicSectorOrganisation.Name;
        public string AddressLine1 => _publicSectorOrganisation.AddressLine1;
        public string AddressLine2 => _publicSectorOrganisation.AddressLine2;
        public string AddressLine3 => _publicSectorOrganisation.AddressLine3;
        public string AddressLine4 => _publicSectorOrganisation.AddressLine4;
        public string AddressLine5 => _publicSectorOrganisation.AddressLine5;
        public string PostCode => _publicSectorOrganisation.PostCode;
    }
}
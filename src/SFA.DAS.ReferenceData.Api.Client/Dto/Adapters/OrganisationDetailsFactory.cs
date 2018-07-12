namespace SFA.DAS.ReferenceData.Api.Client.Dto.Adapters
{
    public class OrganisationDetailsFactory : IOrganisationDetailsFactory
    {
        public IOrganisationDetails Create(PublicSectorOrganisation publicSectorOrganisation)
        {
            return new PublicSectorToOrganisationDetailsAdapter(publicSectorOrganisation);
        }

        public IOrganisationDetails Create(Charity charity)
        {
            return new CharityToNameAndAddressAdapter(charity);
        }

        public IOrganisationDetails Create(Organisation organisation)
        {
            return new OrganisationToNameAndAddressAdapter(organisation);
        }

        public IOrganisationDetails Create(EducationOrganisation educationOrganisation)
        {
            return new EducationOrganisationToNameAndAddressAdapter(educationOrganisation);
        }
    }
}
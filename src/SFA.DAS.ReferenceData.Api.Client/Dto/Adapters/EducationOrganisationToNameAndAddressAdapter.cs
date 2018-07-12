namespace SFA.DAS.ReferenceData.Api.Client.Dto.Adapters
{
    public class EducationOrganisationToNameAndAddressAdapter : IOrganisationDetails
    {
        private readonly EducationOrganisation _educationOrganisation;

        public EducationOrganisationToNameAndAddressAdapter(EducationOrganisation educationOrganisation)
        {
            _educationOrganisation = educationOrganisation;
        }

        public OrganisationType OrganisationType => OrganisationType.EducationOrganisation;
        public string Identifier => _educationOrganisation.Name;
        public string Name => _educationOrganisation.Name;
        public string AddressLine1 => _educationOrganisation.AddressLine1;
        public string AddressLine2 => _educationOrganisation.AddressLine2;
        public string AddressLine3 => _educationOrganisation.AddressLine3;
        public string AddressLine4 => _educationOrganisation.Town;
        public string AddressLine5 => _educationOrganisation.County;
        public string PostCode => _educationOrganisation.PostCode;
    }
}
namespace SFA.DAS.ReferenceData.Api.Client.Dto.Adapters
{
    public interface IOrganisationDetailsFactory
    {
        IOrganisationDetails Create(PublicSectorOrganisation publicSectorOrganisation);
        IOrganisationDetails Create(Charity charity);
        IOrganisationDetails Create(Organisation organisation);
        IOrganisationDetails Create(EducationOrganisation educationOrganisation);
    }
}
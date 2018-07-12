using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.Updater
{
    public interface IPublicSectorOrganisationDatabaseUpdater
    {
        PublicSectorOrganisationLookUp UpdateDatabase(string excelFile);
    }
}

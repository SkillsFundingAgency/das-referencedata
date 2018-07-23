using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.Updater
{
    public interface IJsonManager
    {
        void ExportFile(string filename, PublicSectorOrganisationLookUp orgs);

        void UploadJsonToStorage(string filePath);
    }
}

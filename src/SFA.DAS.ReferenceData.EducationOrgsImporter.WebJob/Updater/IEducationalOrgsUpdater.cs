using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.Updater
{
    public interface IEducationalOrgsUpdater
    {
        Task<PublicSectorOrganisationLookUp> GetData();
    }
}
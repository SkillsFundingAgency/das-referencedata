using System.Threading.Tasks;

namespace SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.Updater
{
    public interface IEducationalOrgsUpdater
    {
        Task RunUpdate();
    }
}
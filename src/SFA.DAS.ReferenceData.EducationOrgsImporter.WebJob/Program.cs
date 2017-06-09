using SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.DependencyResolution;
using SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.Updater;

namespace SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    internal class Program
    {
        internal static void Main(string[] args)
        {
            var container = IoC.Initialize();
            var updater = container.GetInstance<IEducationalOrgsUpdater>();
            updater.RunUpdate().Wait();
        }
    }
}

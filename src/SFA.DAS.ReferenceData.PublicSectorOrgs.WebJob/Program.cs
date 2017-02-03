using SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.DependencyResolution;
using SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.Updater;

namespace SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            var container = IoC.Initialize();
            var updater = container.GetInstance<IPublicOrgsUpdater>();
            updater.RunUpdate().Wait();
        }        
    }
}

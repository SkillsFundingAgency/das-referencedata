using SFA.DAS.ReferenceData.CharityImport.WebJob.DependencyResolution;
using SFA.DAS.ReferenceData.CharityImport.WebJob.Updater;

namespace SFA.DAS.ReferenceData.CharityImport.WebJob
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            var container = IoC.Initialize();
            var updater = container.GetInstance<ICharityImporter>();
            updater.RunUpdate().Wait();

            //var updater = container.GetInstance<INewCharityImporter>();
            //updater.RunUpdate();
        }
    }
}

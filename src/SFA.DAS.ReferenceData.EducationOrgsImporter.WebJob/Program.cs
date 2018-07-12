using System;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.DependencyResolution;
using SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.Updater;

namespace SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    internal class Program
    {
        internal static void Main(string[] args)
        {
            var logger = new NLogLogger(null, null, null);
            try
            {
                logger.Info("WebJob starting");
                var container = IoC.Initialize();

                var updater = container.GetInstance<IEducationalOrgsUpdater>();
                updater.RunUpdate().Wait();
                logger.Info("WebJob finished");
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Unhandled Exception");
            }
        }
    }
}

using System;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.Azure;
using SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.DependencyResolution;
using SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.Serializer;
using SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.Updater;
using SFA.DAS.ReferenceData.Infrastructure.Factories;

namespace SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    internal class Program
    {
        internal static void Main(string[] args)
        {
            try
            {
                var container = IoC.Initialize();

                var updater = container.GetInstance<IEducationalOrgsUpdater>();
                updater.RunUpdate().Wait();
            }
            catch (Exception ex)
            {
                new NLogLogger(null,null,null).Fatal(ex, "Unhandled Exception");
            }
        }
    }
}

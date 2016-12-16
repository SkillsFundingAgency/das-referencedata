using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using SFA.DAS.ReferenceData.CharityImport.WebJob.DependencyResolution;
using SFA.DAS.ReferenceData.CharityImport.WebJob.Updater;

namespace SFA.DAS.ReferenceData.CharityImport.WebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main(string[] args)
        {
           // LoggingConfig.ConfigureLogging();

            var container = IoC.Initialize();

            var updater = container.GetInstance<ICharityImporter>();

            updater.RunUpdate().Wait();
        }
    }
}

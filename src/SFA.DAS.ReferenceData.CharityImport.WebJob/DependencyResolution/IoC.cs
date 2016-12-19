using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Configuration;
using SFA.DAS.ReferenceData.Infrastructure.DependencyResolution;
using StructureMap;

namespace SFA.DAS.ReferenceData.CharityImport.WebJob.DependencyResolution
{
    public static class IoC
    {
        private const string ServiceName = "SFA.DAS.ReferenceData.CharityImporter";

        public static IContainer Initialize()
        {
            return new Container(c =>
            {

                //c.Policies.Add<LoggingPolicy>();
                c.Policies.Add(new ConfigurationPolicy<CharityImporterConfiguration>(ServiceName));
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}

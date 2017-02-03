using SFA.DAS.ReferenceData.Domain.Configuration;
using SFA.DAS.ReferenceData.Infrastructure.DependencyResolution;
using StructureMap;

namespace SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.DependencyResolution
{
    public static class IoC
    {
        private const string ServiceName = "SFA.DAS.ReferenceDataAPI";

        public static IContainer Initialize()
        {
            return new Container(c =>
            {
                c.Policies.Add<LoggingPolicy>();
                c.Policies.Add(new ConfigurationPolicy<ReferenceDataApiConfiguration>(ServiceName));
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}

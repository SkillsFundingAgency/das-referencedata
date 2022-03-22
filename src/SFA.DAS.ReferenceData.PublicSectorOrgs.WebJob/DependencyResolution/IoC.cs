using SFA.DAS.ReferenceData.Infrastructure.DependencyResolution;
using StructureMap;

namespace SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c =>
            {
                c.AddRegistry<DefaultRegistry>();
                c.AddRegistry<ConfigurationRegistry>();
            });
        }
    }
}

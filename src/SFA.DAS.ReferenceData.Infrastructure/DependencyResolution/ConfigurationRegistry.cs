using SFA.DAS.AutoConfiguration;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using SFA.DAS.ReferenceData.Domain.Configuration;
using SFA.DAS.ReferenceData.Domain.Interfaces.Configuration;
using StructureMap;

namespace SFA.DAS.ReferenceData.Infrastructure.DependencyResolution
{
    public class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            IncludeRegistry<AutoConfigurationRegistry>();
            For<ReferenceDataApiConfiguration>().Use(c => c.GetInstance<IAutoConfigurationService>().Get<ReferenceDataApiConfiguration>(ConfigurationKeys.ReferenceDataApi)).Singleton();
            For<IConfiguration>().Use(c => c.GetInstance<IAutoConfigurationService>().Get<ReferenceDataApiConfiguration>(ConfigurationKeys.ReferenceDataApi)).Singleton();
        }
    }
}

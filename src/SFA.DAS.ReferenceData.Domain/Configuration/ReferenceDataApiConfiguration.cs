using SFA.DAS.ReferenceData.Domain.Interfaces.Configuration;

namespace SFA.DAS.ReferenceData.Domain.Configuration
{
    public class ReferenceDataApiConfiguration : IConfiguration
    {
        public string DatabaseConnectionString { get; set; }
    }
}

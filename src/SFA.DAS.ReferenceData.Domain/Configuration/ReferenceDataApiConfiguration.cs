using System.Collections.Generic;
using SFA.DAS.ReferenceData.Domain.Interfaces.Configuration;

namespace SFA.DAS.ReferenceData.Domain.Configuration
{
    public class ReferenceDataApiConfiguration : IConfiguration
    {
        public string DatabaseConnectionString { get; set; }
        public string CharityDataSourceUrl { get; set; }
        public string ONSUrl { get; set; }
        public string PoliceForcesUrl { get; set; }
        public List<string> NhsTrustsUrls { get; set; }
        public string EdubaseUsername { get; set; }
        public string EdubasePassword { get; set; }
        public string OnsUrlDateFormat { get; set; }

        public CompaniesHouseConfiguration CompaniesHouse { get; set; }

    }
}

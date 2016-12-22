﻿using SFA.DAS.ReferenceData.Domain.Interfaces.Configuration;

namespace SFA.DAS.ReferenceData.Domain.Configuration
{
    public class ReferenceDataApiConfiguration : IConfiguration
    {
        public string DatabaseConnectionString { get; set; }
        public string CharityDataSourceUrlPattern { get; set; }
        public string CharityBcpServerName { get; set; }
        public bool CharityBcpTrustedConnection { get; set; }
        public string CharityBcpUsername { get; set; }
        public string CharityBcpPassword { get; set; }
        public string CharityBcpTargetDb { get; set; }
        public string CharityBcpTargetSchema { get; set; }
        public string CharityBcpRowTerminator { get; set; }
        public string CharityBcpFieldTerminator { get; set; }

    }
}

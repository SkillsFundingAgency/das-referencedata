using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using FastMember;
using Microsoft.Azure.Services.AppAuthentication;
using SFA.DAS.ReferenceData.Domain.Interfaces.Configuration;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Models.Charity;
using SFA.DAS.ReferenceData.Domain.Models.Data;

namespace SFA.DAS.ReferenceData.Infrastructure.Data
{
    public class CharityImportRepository : BaseRepository, ICharityImportRepository
    {
        private const string AzureResource = "https://database.windows.net/";
        private readonly string _connectionString;
        private readonly string _environment;

        public CharityImportRepository(IConfiguration configuration) : base(configuration)
        {
            _connectionString = configuration.DatabaseConnectionString;
            _environment = ConfigurationManager.AppSettings["EnvironmentName"];
        }

        public async Task ImportToStagingTable(IEnumerable<CharityImport> charityImports)
        {           
            using (var connection = OpenConnection())
            using (var bulkCopy = new SqlBulkCopy(connection))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                bulkCopy.BatchSize = 1000;
                bulkCopy.BulkCopyTimeout = 3600;
                bulkCopy.DestinationTableName = "[CharityImport].[extract_charity_import]";
                bulkCopy.ColumnMappings.Clear();
                
                PopulateBulkCopy(bulkCopy, typeof(CharityImport));

                using (var reader = ObjectReader.Create(charityImports))
                {
                    await bulkCopy.WriteToServerAsync(reader).ConfigureAwait(false);
                }
            }          
        }

        private void PopulateBulkCopy(SqlBulkCopy bulkCopy, Type entityType)
        {
            var columns = entityType.GetProperties();
            foreach (var propertyInfo in columns)
            {
                bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(propertyInfo.Name, propertyInfo.Name));
            }
        }

        private SqlConnection OpenConnection()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var environment = GetEnvironment();

            return environment.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase)
                    ? new SqlConnection(_connectionString)
                    : new SqlConnection
                    {
                        ConnectionString = _connectionString,
                        AccessToken = azureServiceTokenProvider.GetAccessTokenAsync(AzureResource).Result
                    };
        }

        private string GetEnvironment()
        {
            var environment = Environment.GetEnvironmentVariable("DASENV");
            if (string.IsNullOrEmpty(environment))
            {
                environment = _environment;
            }

            return environment;
        }
    }
}

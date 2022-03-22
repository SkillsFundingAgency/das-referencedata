using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using FastMember;
using SFA.DAS.ReferenceData.Domain.Interfaces.Configuration;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Models.Charity;
using SFA.DAS.ReferenceData.Domain.Models.Data;

namespace SFA.DAS.ReferenceData.Infrastructure.Data
{
    public class CharityImportRepository : BaseRepository, ICharityImportRepository
    {
        private readonly string _connectionString;

        public CharityImportRepository(IConfiguration configuration) : base(configuration)
        {
            _connectionString = configuration.DatabaseConnectionString;
        }

        public async Task ImportToStagingTable(IEnumerable<CharityImport> charityImports)
        {           
            using (var connection = new SqlConnection(_connectionString))
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
    }
}

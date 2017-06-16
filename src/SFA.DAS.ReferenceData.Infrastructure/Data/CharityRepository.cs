using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ReferenceData.Domain.Interfaces.Configuration;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Models.Charity;
using SFA.DAS.ReferenceData.Domain.Models.Data;

namespace SFA.DAS.ReferenceData.Infrastructure.Data
{
    public class CharityRepository : BaseRepository, ICharityRepository
    {
        public CharityRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<CharityDataImport> GetLastCharityDataImport()
        {
            var result = await WithConnection(async c => await c.QueryAsync<CharityDataImport>(
                sql: "[CharityData].[GetLastCharityDataImport]",
                commandType: CommandType.StoredProcedure));
            return result.SingleOrDefault();
        }

        public async Task RecordCharityDataImport(int month, int year)
        {
            var result = await WithConnection(async c => await c.ExecuteAsync(
                sql: "[CharityData].[CreateCharityDataImport]",
                commandType: CommandType.StoredProcedure));
        }

        public async Task TruncateLoadTables()
        {
            var result = await WithConnection(async c => await c.ExecuteAsync(
                sql: "[CharityData].[TruncateLoadTables]",
                commandType: CommandType.StoredProcedure));
        }

        public async Task CreateCharityDataImport(int month, int year)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@month", month, DbType.Int32);
            parameters.Add("@year", year, DbType.Int32);

            var result = await WithConnection(async c => await c.ExecuteAsync(
                sql: "[CharityData].[CreateCharityDataImport]",
                param: parameters,
                commandType: CommandType.StoredProcedure));
        }

        public async Task ImportDataFromLoadTables()
        {
            var result = await WithConnection(async c => await c.ExecuteAsync(
                sql: "[CharityData].[ImportDataFromLoadTables]",
                commandType: CommandType.StoredProcedure,
                commandTimeout: 3600
                ));
        }

        public async Task<Charity> GetCharityByRegistrationNumber(int registrationNumber)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@RegistrationNumber", registrationNumber, DbType.Int32);

            var result = await WithConnection(async c => await c.QueryAsync<Charity>(
                sql: "[CharityData].[GetCharityByRegistrationNumber]",
                param: parameters,
                commandType: CommandType.StoredProcedure));
            return result.SingleOrDefault();
        }

        public async Task<IEnumerable<Charity>> FindCharities(string searchTerm, int pageSize, int pageNumber)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SearchTerm", searchTerm, DbType.String);

            var result = await WithConnection(async c => await c.QueryAsync<Charity>(
                sql: "[CharityData].[FindCharities]",
                param: parameters,
                commandType: CommandType.StoredProcedure));
            return result;
        }
    }
}

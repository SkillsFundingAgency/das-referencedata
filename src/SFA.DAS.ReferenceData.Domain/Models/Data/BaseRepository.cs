using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using SFA.DAS.ReferenceData.Domain.Interfaces.Configuration;

namespace SFA.DAS.ReferenceData.Domain.Models.Data
{
    public abstract class BaseRepository
    {
        private const string AzureResource = "https://database.windows.net/";
        private readonly string _connectionString;
        private readonly string _environment;

        protected BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.DatabaseConnectionString;
            //_environment = ConfigurationSettings.AppSettings.Get("EnvironmentName");
            _environment = ConfigurationManager.AppSettings["EnvironmentName"];
        }

        protected async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            using (var connection = OpenConnection())
            {
                await connection.OpenAsync();
                return await getData(connection);
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

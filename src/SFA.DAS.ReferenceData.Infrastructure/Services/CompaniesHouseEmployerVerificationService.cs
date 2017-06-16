using System;
using System.Threading.Tasks;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Configuration;
using SFA.DAS.ReferenceData.Domain.Http;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models.Company;
using SFA.DAS.ReferenceData.Infrastructure.ExecutionPolicies;

namespace SFA.DAS.ReferenceData.Infrastructure.Services
{
    public class CompaniesHouseEmployerVerificationService : ICompaniesHouseEmployerVerificationService
    {
        private readonly ReferenceDataApiConfiguration _configuration;
        private readonly ILog _logger;
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly ExecutionPolicy _executionPolicy;

        public CompaniesHouseEmployerVerificationService(ReferenceDataApiConfiguration configuration, ILog logger, IHttpClientWrapper httpClientWrapper,
            [RequiredPolicy(CompaniesHouseExecutionPolicy.Name)]ExecutionPolicy executionPolicy)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClientWrapper = httpClientWrapper;
            _executionPolicy = executionPolicy;
            _httpClientWrapper.AuthScheme = "Basic";
            _httpClientWrapper.BaseUrl = _configuration.CompaniesHouse.BaseUrl;
        }

        public async Task<EmployerInformation> GetInformation(string id)
        {
            return await _executionPolicy.ExecuteAsync(async () =>
            {
                _logger.Info($"GetInformation({id})");

                id = id?.ToUpper();

                var result = await _httpClientWrapper.Get<EmployerInformation>(
                    $"{Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(_configuration.CompaniesHouse.ApiKey))}",
                    $"{_configuration.CompaniesHouse.BaseUrl}/company/{id}");
                return result;
            });
        }
    }
}

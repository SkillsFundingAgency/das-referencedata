using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models.Data;
using SFA.DAS.ReferenceData.Domain.Models.Education;
using SFA.DAS.ReferenceData.Infrastructure.Caching;
using SFA.DAS.ReferenceData.Types.DTO;

namespace SFA.DAS.ReferenceData.Infrastructure.Data
{
    public class EducationalOrganisationRepository : IEducationalOrganisationRepository
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly IAzureService _azureService;
        private readonly ILog _logger;

        private const string ContainerName = "sfa-das-reference-data";
        private const string BlobName = "EducationalOrganisations.json";

        public EducationalOrganisationRepository(ICacheProvider cacheProvider, IAzureService azureService,
            ILog logger)
        {
            _cacheProvider = cacheProvider;
            _azureService = azureService;
            _logger = logger;
        }

        public async Task<PagedResult<EducationOrganisation>> FindOrganisations(string searchTerm, int pageSize, int pageNumber)
        {
            return await Task.Run(async () =>
            {
                var lookUp = await _cacheProvider.GetAsync<EducationalOrganisationLookUp>(nameof(EducationalOrganisationLookUp));

                if (lookUp == null)
                {
                    _logger.Info("Getting educational orgainsations from Azure storage as cache is empty.");
                    lookUp = await _azureService.GetModelFromBlobStorage<EducationalOrganisationLookUp>(ContainerName,
                        BlobName);

                    if (lookUp == null)
                    {
                        _logger.Info("No educational organisations were retrieved from Azure.");
                        return new PagedResult<EducationOrganisation>
                        {
                            Data = new List<EducationOrganisation>()
                        };
                    }

                    _logger.Info(
                        $"{lookUp.Organisations.Count()} educational organisations were retrieved from Azure");

                    await _cacheProvider.SetAsync(nameof(EducationalOrganisationLookUp), lookUp, TimeSpan.FromDays(14));
                    _logger.Info($"Cached educational organisations till {DateTime.Now.AddDays(1):R}");
                }

                pageSize = pageSize < 1 ? 1 : pageSize;

                var organisations = lookUp.Organisations.ToList();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    organisations = organisations
                        .Where(o => o.Name.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) >= 0)
                        .ToList();
                }

                var totalPages = organisations.Count % pageSize;


                var offset = GetPageOffset(pageSize, pageNumber, organisations.Count);

                organisations = organisations.Skip(offset)
                    .Take(pageSize)
                    .ToList();

                return new PagedResult<EducationOrganisation>
                {
                    Data = organisations,
                    Page = pageNumber,
                    TotalPages = totalPages
                };
            });
        }

        private static int GetPageOffset(int pageSize, int pageNumber, int organisationCount)
        {
            var maxOffset = organisationCount - pageSize;

            var offset = pageNumber < 2 ? 0 : pageNumber * pageSize;

            return offset > maxOffset ? maxOffset : offset;
        }
    }
}
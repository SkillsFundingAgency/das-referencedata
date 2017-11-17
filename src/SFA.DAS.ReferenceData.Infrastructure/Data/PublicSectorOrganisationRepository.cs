using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Interfaces.Caching;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models;
using SFA.DAS.ReferenceData.Domain.Models.Data;
using SFA.DAS.ReferenceData.Infrastructure.Caching;

namespace SFA.DAS.ReferenceData.Infrastructure.Data
{
    public class PublicSectorOrganisationRepository : IPublicSectorOrganisationRepository, ICachedRepository
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly IAzureService _azureService;
        private readonly ILog _logger;

        private const string ContainerName = "sfa-das-reference-data";
        private const string BlobName = "PublicOrganisationNames.json";

        public PublicSectorOrganisationRepository(ICacheProvider cacheProvider, IAzureService azureService, ILog logger)
        {
            _cacheProvider = cacheProvider;
            _azureService = azureService;
            _logger = logger;
        }

        public async Task<PagedResult<PublicSectorOrganisation>> FindOrganisations(string searchTerm, int pageSize, int pageNumber)
        {
            return await Task.Run(async() =>
            {
                var lookUp = await _cacheProvider.GetAsync<PublicSectorOrganisationLookUp>(nameof(PublicSectorOrganisationLookUp));

                if (lookUp == null)
                {
                    await RefreshCache();

                    lookUp = await _cacheProvider.GetAsync<PublicSectorOrganisationLookUp>(nameof(PublicSectorOrganisationLookUp));

                    if (lookUp == null)
                    {
                        _logger.Info("No public sector organisations were retrieved from Azure.");
                        return new PagedResult<PublicSectorOrganisation>
                        {
                            Data = new List<PublicSectorOrganisation>()
                        };
                    }

                    _logger.Info($"{lookUp.Organisations.Count()} public sector organisations were retrieved from Azure");
                }

                pageSize = pageSize < 1 ? 1 : pageSize;

                var organisations = lookUp.Organisations.ToList();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    organisations = organisations.Where(o => o.Name.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) >= 0).ToList();
                }

                var totalPages = organisations.Count % pageSize;


                var offset = GetPageOffset(pageSize, pageNumber, organisations.Count);

                organisations = organisations.Skip(offset)
                                             .Take(pageSize)
                                             .ToList();

                return new PagedResult<PublicSectorOrganisation>
                {
                    Data = organisations,
                    Page = pageNumber,
                    TotalPages = totalPages
                };
            });
        }

        public async Task RefreshCache()
        {
            _logger.Info("Refreshing public sector orgainsations Azure storage cache.");

            var lookUp = await _azureService.GetModelFromBlobStorage<PublicSectorOrganisationLookUp>(ContainerName, BlobName);
            await _cacheProvider.SetAsync(nameof(PublicSectorOrganisationLookUp), lookUp, TimeSpan.FromDays(14));

            _logger.Info($"Cached public sector organisations till {DateTime.Now.AddDays(14):R}");
        }

        private static int GetPageOffset(int pageSize, int pageNumber, int organisationCount)
        {
            var maxOffset = organisationCount - pageSize;

            var offset = pageNumber < 2 ? 0 : pageNumber * pageSize;

            return offset > maxOffset ? maxOffset : offset;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models;
using SFA.DAS.ReferenceData.Infrastructure.Caching;

namespace SFA.DAS.ReferenceData.Infrastructure.Data
{
    public class PublicSectorOrganisationRepository : IPublicSectorOrganisationRepository
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly IAzureService _azureService;
        private readonly ILogger _logger;

        private const string ContainerName = "sfa-das-reference-data";
        private const string BlobName = "PublicOrganisationNames.json";

        public PublicSectorOrganisationRepository(ICacheProvider cacheProvider, IAzureService azureService, ILogger logger)
        {
            _cacheProvider = cacheProvider;
            _azureService = azureService;
            _logger = logger;
        }

        public async Task<ICollection<PublicSectorOrganisation>> FindOrganisations(string searchTerm, int pageSize, int pageNumber)
        {
            return await Task.Run(async() =>
            {
                var lookUp = _cacheProvider.Get<PublicSectorOrganisationLookUp>(nameof(PublicSectorOrganisationLookUp));

                if (lookUp == null)
                {
                    _logger.Info("Getting public sector orgainsations from Azure storage as cache is empty.");
                    lookUp = await _azureService.GetModelFromBlobStorage<PublicSectorOrganisationLookUp>(ContainerName, BlobName);
                    
                    if (lookUp == null)
                    {
                        _logger.Info("No public sector organisations were retrieved from Azure.");
                        return new List<PublicSectorOrganisation>();
                    }

                    _logger.Info($"{lookUp.OrganisationNames.Count()} public sector organisations were retrieved from Azure");

                    _cacheProvider.Set(nameof(PublicSectorOrganisationLookUp), lookUp, TimeSpan.FromDays(14));
                    _logger.Info($"Cached public sector organisations till {DateTime.Now.AddDays(14):R}");
                }

                pageSize = pageSize < 1 ? 1 : pageSize;

                var organisations = lookUp.OrganisationNames.ToList();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    organisations = organisations.Where(name => name.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) >= 0).ToList();
                }

                var offset = GetPageOffset(pageSize, pageNumber, organisations.Count);

                organisations = organisations.Skip(offset)
                                             .Take(pageSize)
                                             .ToList();

                var selectedOrgainsations = organisations.Select(name =>
                    new PublicSectorOrganisation
                    {
                        Name = name
                    }).ToList();

                return selectedOrgainsations;
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

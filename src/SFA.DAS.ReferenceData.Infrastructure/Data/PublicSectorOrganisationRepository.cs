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
    public class PublicSectorOrganisationRepository : IPubicSectorOrganisationRepository
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

        public async Task<ICollection<PublicSectorOrganisation>> GetOrganisations()
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

                var organisations = lookUp.OrganisationNames
                    .Select(name => new PublicSectorOrganisation { Name = name })
                    .ToList();

                return organisations;
            });
        }
    }
}

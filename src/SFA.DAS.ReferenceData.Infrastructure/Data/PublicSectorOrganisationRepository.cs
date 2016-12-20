using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private const string ContainerName = "sfa-das-reference-data";
        private const string BlobName = "PublicOrganisationNames.json";

        public PublicSectorOrganisationRepository(ICacheProvider cacheProvider, IAzureService azureService)
        {
            _cacheProvider = cacheProvider;
            _azureService = azureService;
        }

        public async Task<ICollection<PublicSectorOrganisation>> GetOrganisations()
        {
            return await Task.Run(async() =>
            {
                var lookUp = _cacheProvider.Get<PublicSectorOrganisationLookUp>(nameof(PublicSectorOrganisationLookUp));

                if (lookUp == null)
                {
                    lookUp =  await _azureService.GetModelFromBlobStorage<PublicSectorOrganisationLookUp>(ContainerName, BlobName);

                    if (lookUp == null)
                        return new List<PublicSectorOrganisation>();

                    _cacheProvider.Set(nameof(PublicSectorOrganisationLookUp), lookUp, TimeSpan.FromDays(14));
                }

                var organisations = lookUp.OrganisationNames
                    .Select(name => new PublicSectorOrganisation { Name = name })
                    .ToList();

                return organisations;
            });
        }
    }
}

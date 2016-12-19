using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models;
using SFA.DAS.ReferenceData.Infrastructure.Caching;

namespace SFA.DAS.ReferenceData.Infrastructure.Services
{
    public class PublicSectorOrganisationLookUpService : AzureServiceBase<PublicSectorOrganisationLookUp>, IPublicSectorOrganisationLookUpService
    {
        private readonly ICacheProvider _cacheProvider;

        private const string ContainerName = "sfa-das-reference-data";
        private const string BlobName = "PublicOrganisationNames.json";

        public PublicSectorOrganisationLookUpService(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public async Task<ICollection<PublicSectorOrganisation>> GetOrganisations()
        {
            return await Task.Run(() =>
            {
                var lookUp = _cacheProvider.Get<PublicSectorOrganisationLookUp>(nameof(PublicSectorOrganisationLookUp));

                if (lookUp == null)
                {
                    lookUp = GetDataFromBlobStorage(ContainerName, BlobName);

                    if(lookUp == null)
                        return new List<PublicSectorOrganisation>();

                    _cacheProvider.Set(nameof(PublicSectorOrganisationLookUp), lookUp, TimeSpan.FromDays(14));
                }
                
                var organisations = lookUp.OrganisationNames
                    .Select(name => new PublicSectorOrganisation {Name = name})
                    .ToList();

                return organisations;
            });
        }
    }
}

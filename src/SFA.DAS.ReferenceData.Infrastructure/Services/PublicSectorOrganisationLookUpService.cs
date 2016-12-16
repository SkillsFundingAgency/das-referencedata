using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.Infrastructure.Services
{
    public class PublicSectorOrganisationLookUpService : AzureServiceBase<PublicSectorOrganisationLookUp>, IPublicSectorOrganisationLookUpService
    {
        public override string ConfigurationName { get; } = "SFA.DAS.ReferenceData.PublicOrganisationNames";

        public PublicSectorOrganisationLookUpService() //ICacheProvider cacheProvider
        {
            
        }

        public async Task<ICollection<PublicSectorOrganisation>> GetOrganisations()
        {
            return await Task.Run(() =>
            {
                var lookUp = GetDataFromStorage();

                var organisations = lookUp.OrganisationNames
                    .Select(name => new PublicSectorOrganisation {Name = name})
                    .ToList();

                return organisations;
            });
        }
    }
}

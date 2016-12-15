using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Interfaces.Configuration;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Models;
using SFA.DAS.ReferenceData.Domain.Models.Data;

namespace SFA.DAS.ReferenceData.Infrastructure.Data
{
    public class OrganisationRepository : BaseRepository, IOrganisationRepository
    {
        public OrganisationRepository(IConfiguration configuration) : base(configuration)
        {
        }
        
        public async Task<IEnumerable<PublicSectorOrganisation>> GetPublicSectorOrganisations()
        {
           throw new NotImplementedException();
        }
    }
}

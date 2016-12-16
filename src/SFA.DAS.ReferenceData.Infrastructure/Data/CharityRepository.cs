using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Interfaces.Configuration;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;using SFA.DAS.ReferenceData.Domain.Models.Data;

namespace SFA.DAS.ReferenceData.Infrastructure.Data
{
    public class CharityRepository : BaseRepository, ICharityRepository
    {
        public CharityRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task TruncateLoadTables()
        {
            throw new NotImplementedException();
        }
    }
}

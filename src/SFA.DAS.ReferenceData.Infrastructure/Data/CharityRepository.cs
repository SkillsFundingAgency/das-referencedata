using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Interfaces.Configuration;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Models.Charity;
using SFA.DAS.ReferenceData.Domain.Models.Data;

namespace SFA.DAS.ReferenceData.Infrastructure.Data
{
    //todo: put this back
    //public class CharityRepository : BaseRepository, ICharityRepository
    public class CharityRepository : ICharityRepository
    {
        //public CharityRepository(IConfiguration configuration) : base(configuration)
        //{
        //}

        public async Task<CharityDataImport> GetLastCharityDataImport()
        {
            throw new NotImplementedException();
        }

        public Task RecordCharityDataImport(int month, int year)
        {
            throw new NotImplementedException();
        }

        public async Task TruncateLoadTables()
        {
            throw new NotImplementedException();
        }
    }
}

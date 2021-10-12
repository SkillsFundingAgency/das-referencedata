using SFA.DAS.ReferenceData.Domain.Models.Charity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Data
{
    public interface ICharityImportRepository
    {      
        Task ImportToStagingTable(IEnumerable<CharityImport> charityImports);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Models.Charity;
using SFA.DAS.ReferenceData.Types;
using SFA.DAS.ReferenceData.Types.DTO;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Data
{
    public interface ICharityRepository
    {
        Task<CharityDataImport> GetLastCharityDataImport();
        Task RecordCharityDataImport(int month, int year);
        Task TruncateLoadTables();
        Task CreateCharityDataImport(int month, int year);
        Task ImportDataFromLoadTables();
        Task ImportFromPublicExtractCharityToStagingTable();
        Task<Charity> GetCharityByRegistrationNumber(int registrationNumber);
        Task<IEnumerable<Charity>> FindCharities(string searchTerm, int maximumResults);
    }
}

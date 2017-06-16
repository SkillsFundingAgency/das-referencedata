using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Models.Charity;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Data
{
    public interface ICharityRepository
    {
        Task<CharityDataImport> GetLastCharityDataImport();
        Task RecordCharityDataImport(int month, int year);
        Task TruncateLoadTables();
        Task CreateCharityDataImport(int month, int year);
        Task ImportDataFromLoadTables();
        Task<Charity> GetCharityByRegistrationNumber(int registrationNumber);
        Task<IEnumerable<Charity>> FindCharities(string searchTerm, int maximumResults);
    }
}

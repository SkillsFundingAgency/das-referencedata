using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Models.Company;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Services
{
    public interface ICompaniesHouseEmployerVerificationService
    {
        Task<CompanyInformation> GetInformation(string id);

        Task<CompanySearchResults> FindCompany(string searchTerm);
    }
}
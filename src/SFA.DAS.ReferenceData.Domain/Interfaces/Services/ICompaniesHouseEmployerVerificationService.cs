using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Models.Company;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Services
{
    public interface ICompaniesHouseEmployerVerificationService
    {
        Task<EmployerInformation> GetInformation(string id);
    }
}
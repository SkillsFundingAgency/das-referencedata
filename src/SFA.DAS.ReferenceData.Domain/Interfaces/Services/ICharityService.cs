using SFA.DAS.ReferenceData.Domain.Models.Bcp;
using System.Threading.Tasks;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Services
{
    public interface ICharityService
    {
        Task ExecuteCharityImport(string sourceDirectory);
    }
}

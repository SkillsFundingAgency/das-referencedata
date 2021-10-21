using System.IO;
using System.Threading.Tasks;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Services
{
    public interface ICharityService
    {
        Task ExecuteCharityImport(Stream content);
    }
}

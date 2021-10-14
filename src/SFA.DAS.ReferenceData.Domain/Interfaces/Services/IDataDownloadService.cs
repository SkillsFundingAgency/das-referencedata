using System.IO;
using System.Threading.Tasks;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Services
{
    public interface IDataDownloadService
    {
        Task<Stream> GetFileStream(string downloadPath);
    }
}

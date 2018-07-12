using System.Threading.Tasks;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Services
{
    public interface IArchiveDownloadService
    {
        Task<bool> DownloadFile(string url, string targetPath, string targetFilename);

        void UnzipFile(string zipFile, string targetPath);

    }
}

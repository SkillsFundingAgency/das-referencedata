using System.IO;
using System.Linq;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;

namespace SFA.DAS.ReferenceData.Infrastructure.Data
{
    public class FileSystemRepository : IFileSystemRepository
    {
        public string GetDataFile(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return "";
            }
            return Directory.GetFiles(directory, "*.csv").FirstOrDefault();
        }

        public void DeleteFile(string fileFullName)
        {
            if (File.Exists(fileFullName))
            {
                File.Delete(fileFullName);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Services
{
    public interface IArchiveDownloadService
    {
        Task<bool> DownloadFile(string url, string targetPath, string targetFilename);

        bool UnzipFile(string zipFile, string targetPath);

    }
}

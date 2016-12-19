using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NLog.Targets;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using System.IO.Compression;

namespace SFA.DAS.ReferenceData.Infrastructure.Services
{
    public class ArchiveDownloadService : IArchiveDownloadService
    {
        public async Task<bool> DownloadFile(string url, string targetPath, string targetFilename)
        {
            var filenameAndPath = Path.Combine(targetPath, targetFilename);

            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return false;
                    }

                    using (var streamToReadFrom = await response.Content.ReadAsStreamAsync())
                    {
                        using (Stream streamToWriteTo = File.Open(filenameAndPath, FileMode.Create, FileAccess.ReadWrite))
                        {
                            await streamToReadFrom.CopyToAsync(streamToWriteTo);
                        }
                    }
                }
            }
            return true;
        }

        public bool UnzipFile(string zipFile, string targetPath)
        {
            var fileInfo = new FileInfo(zipFile);

            ZipFile.ExtractToDirectory(zipFile, targetPath);
            return true;
        }
    }
}

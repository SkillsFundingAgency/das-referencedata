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
using NLog;

namespace SFA.DAS.ReferenceData.Infrastructure.Services
{
    public class ArchiveDownloadService : IArchiveDownloadService
    {
        private readonly ILogger _logger;

        public ArchiveDownloadService(ILogger logger)
        {
            _logger = logger;
        }

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

        public void UnzipFile(string zipFile, string targetPath)
        {
            var dirInfo = new DirectoryInfo(targetPath);
            if (dirInfo.Exists)
            {
                _logger.Warn($"Extract folder {targetPath} already exists - deleting");
                try
                {
                    dirInfo.Delete(true);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error deleting folder");
                    throw;
                }
            }
            
            try
            {
                _logger.Warn($"Extracting {zipFile} to {targetPath}");
                ZipFile.ExtractToDirectory(zipFile, targetPath);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error extracting archive");
                throw;
            }
        }
    }
}

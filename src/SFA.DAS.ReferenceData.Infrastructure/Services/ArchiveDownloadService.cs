using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using System.IO.Compression;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.ReferenceData.Infrastructure.Services
{
    public class ArchiveDownloadService : IArchiveDownloadService
    {
        private readonly ILog _logger;

        public ArchiveDownloadService(ILog logger)
        {
            _logger = logger;
        }

        public async Task<bool> DownloadFile(string url, string targetPath, string targetFilename)
        {
            Directory.CreateDirectory(targetPath);

            var filenameAndPath = Path.Combine(targetPath, targetFilename);

            _logger.Info($"Downloading {url} to {filenameAndPath}...");

            using (var client = new HttpClient())
            {
                // Add a User-Agent header to mimic a web browser
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
                
                using (var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.Error(new Exception($"Status code {response.StatusCode} returned"), $"Status code {response.StatusCode} returned");
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
            _logger.Info("Download complete");
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

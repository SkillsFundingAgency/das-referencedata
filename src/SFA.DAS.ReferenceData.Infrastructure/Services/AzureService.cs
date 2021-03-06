﻿using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;

namespace SFA.DAS.ReferenceData.Infrastructure.Services
{
    public class AzureService : IAzureService
    {
        private readonly ILog _logger;

        public AzureService(ILog logger)
        {
            _logger = logger;
        }

        public async Task<T> GetModelFromBlobStorage<T>(string containerName, string blobName)
        {
            var blobData = await GetBlobDataFromAzure(containerName, blobName);

            if (blobData == null)
            {
                return default(T);
            }

            using (var stream = new MemoryStream(blobData))
            {
                using (var reader = new StreamReader(stream))
                {
                    var jsonContent = reader.ReadToEnd();

                    return string.IsNullOrEmpty(jsonContent)
                        ? default(T)
                        : JsonConvert.DeserializeObject<T>(jsonContent);
                }
            }
        }

        private async Task<byte[]> GetBlobDataFromAzure(string containerName, string blobName)
        {
            try
            {
                var storageAccount =
                    CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

                var client = storageAccount.CreateCloudBlobClient();

                var container = client.GetContainerReference(containerName);

                var blob = container.GetBlobReference(blobName);

                using (var stream = new MemoryStream())
                {
                    await blob.DownloadRangeToStreamAsync(stream, 0, null);

                    return stream.ToArray();
                }
            }
            catch (StorageException e)
            {
                _logger.Warn(e, "Unable to get blob from azure storage");
            }

            return null;
        }
    }
}
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using NLog;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.Azure
{
    public class AzureStorageUploader : IAzureStorageUploader
    {
        private readonly ILog _logger;
        private const string JsonContainerName = "sfa-das-reference-data";
        private const string JsonFileName = "EducationalOrganisations.json";

        public AzureStorageUploader(ILog logger)
        {
            _logger = logger;
        }

        public async Task UploadDataToStorage(byte[] data)
        {
            _logger.Info("Uploading educational organisations to Blob storage");

            try
            {
                var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

                var blobClient = storageAccount.CreateCloudBlobClient();
                
                var container = blobClient.GetContainerReference(JsonContainerName);
                container.CreateIfNotExists();

                var blockBlob = container.GetBlockBlobReference(JsonFileName);

                using (var stream = new MemoryStream(data))
                {
                    await blockBlob.UploadFromStreamAsync(stream);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error uploading Educational organisations to Blob storage");
                throw new Exception("Error uploading Educational organisations to Blob storage", e);
            }
        }
    }
}

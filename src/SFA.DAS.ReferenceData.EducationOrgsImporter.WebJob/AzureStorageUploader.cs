using System;
using System.Configuration;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using NLog;


namespace SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob
{
    class AzureStorageUploader
    {
        private readonly ILogger _logger;
        private const string JsonContainerName = "sfa-das-reference-data";
        private const string JsonFileName = "EducationalOrganisations.json";


        public AzureStorageUploader(ILogger logger)
        {
            _logger = logger;
        }

        public void UploadDataToStorage(byte[] data)
        {
            _logger.Info($"Uploading educational organisations to Blob storage");

            try
            {
                var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference(JsonContainerName);
                container.CreateIfNotExists();

                var blockBlob = container.GetBlockBlobReference(JsonFileName);

                using (var stream = new MemoryStream(data))
                {
                    blockBlob.UploadFromStream(stream);
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

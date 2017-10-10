using System;
using System.Configuration;
using System.IO;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.Updater
{
    public class JsonManager : IJsonManager
    {
        private readonly ILog _logger;
        private readonly string _jsonContainerName = "sfa-das-reference-data";
        private readonly string _jsonFileName = "PublicOrganisationNames.json";

        public JsonManager(ILog logger)
        {
            _logger = logger;
        }

        public void ExportFile(string filename, PublicSectorOrganisationLookUp orgs)
        {
            try
            {
                using (var fs = File.Open(filename, FileMode.Create))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        using (var jw = new JsonTextWriter(sw))
                        {
                            jw.Formatting = Formatting.Indented;

                            var serializer = new JsonSerializer();
                            serializer.Serialize(jw, orgs);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"An error occurred exporting data to {filename}: {ex.Message}");
                throw;
            }

            Console.WriteLine($"Exported {orgs.Organisations.Count()} records");
        }

        public void UploadJsonToStorage(string filePath)
        {
            _logger.Info($"Uploading {filePath} to Blob storage");

            try
            {
                var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference(_jsonContainerName);
                container.CreateIfNotExists();

                var blockBlob = container.GetBlockBlobReference(_jsonFileName);

                using (var fileStream = File.OpenRead(filePath))
                {
                    blockBlob.UploadFromStream(fileStream);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error uploading {filePath} to Blob storage");
                throw;
            }
        }
    }
}
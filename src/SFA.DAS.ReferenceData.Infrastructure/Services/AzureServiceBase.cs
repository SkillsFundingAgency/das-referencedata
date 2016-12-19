using System.IO;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace SFA.DAS.ReferenceData.Infrastructure.Services
{
    public abstract class AzureServiceBase<T>
    {
        private readonly CloudStorageAccount _storageAccount;

        protected AzureServiceBase()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
        }

        public virtual T GetDataFromBlobStorage(string containerName, string blobName)
        {
            var client = _storageAccount.CreateCloudBlobClient();

            var container = client.GetContainerReference(containerName);

            var blob = container.GetBlobReference(blobName);

            string jsonContent;

            using (var stream = new MemoryStream())
            {
                blob.DownloadToStream(stream);

                stream.Position = 0;

                using (var reader = new StreamReader(stream))
                {
                    jsonContent = reader.ReadToEnd();
                }
            }

            return string.IsNullOrEmpty(jsonContent) ? default(T) : JsonConvert.DeserializeObject<T>(jsonContent);
        }
    }
}
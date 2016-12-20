using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;

namespace SFA.DAS.ReferenceData.Infrastructure.Services
{
    public class AzureService : IAzureService
    {
        public async Task<T> GetModelFromBlobStorage<T>(string containerName, string blobName)
        {
            var blobData = await GetBlobDataFromAzure(containerName, blobName);

            using (var stream = new MemoryStream(blobData))
            {
                using (var reader = new StreamReader(stream))
                {
                    var jsonContent = reader.ReadToEnd();

                    return string.IsNullOrEmpty(jsonContent) ? default(T) : JsonConvert.DeserializeObject<T>(jsonContent);
                }
            }
        }

        private static async Task<byte[]> GetBlobDataFromAzure(string containerName, string blobName)
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            var client = storageAccount.CreateCloudBlobClient();

            var container = client.GetContainerReference(containerName);

            var blob = container.GetBlobReference(blobName);

            using (var stream = new MemoryStream())
            {
                await blob.DownloadRangeToStreamAsync(stream, 0, null);

                return stream.ToArray();
            }
        }
    }
}
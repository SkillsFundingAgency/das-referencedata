using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;

namespace SFA.DAS.ReferenceData.Infrastructure.Services
{
    public class DataDownloadService : IDataDownloadService
    {
        public async Task<Stream> GetFileStream(string downloadPath)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(downloadPath);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                return stream;
            }
        }
    }
}

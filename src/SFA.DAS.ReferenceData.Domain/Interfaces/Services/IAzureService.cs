using System.Threading.Tasks;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Services
{
    public interface IAzureService
    {
        Task<T> GetModelFromBlobStorage<T>(string containerName, string blobName);
    }
}

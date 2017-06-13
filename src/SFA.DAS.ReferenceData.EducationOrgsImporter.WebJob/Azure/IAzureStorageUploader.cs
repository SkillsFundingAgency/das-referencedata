using System.Threading.Tasks;

namespace SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.Azure
{
    public interface IAzureStorageUploader
    {
        Task UploadDataToStorage(byte[] data);
    }
}
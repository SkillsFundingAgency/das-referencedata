namespace SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.Azure
{
    public interface IAzureStorageUploader
    {
        void UploadDataToStorage(byte[] data);
    }
}
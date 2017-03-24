namespace SFA.DAS.ReferenceData.Domain.Interfaces.Data
{
    public interface IFileSystemRepository
    {
        string GetDataFile(string directory);
        void DeleteFile(string fileFullName);
    }
}
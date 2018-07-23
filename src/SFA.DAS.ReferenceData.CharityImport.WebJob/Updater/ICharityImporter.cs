using System.Threading.Tasks;

namespace SFA.DAS.ReferenceData.CharityImport.WebJob.Updater
{
    public interface ICharityImporter
    {
        Task RunUpdate();
    }
}

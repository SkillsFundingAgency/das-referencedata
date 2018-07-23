using System.Threading.Tasks;

namespace SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.Updater
{
    public interface IPublicOrgsUpdater
    {
        Task RunUpdate();
    }
}

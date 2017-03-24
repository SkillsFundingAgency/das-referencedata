using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.Updater
{
    public interface INhsDataUpdater
    {
        Task<PublicSectorOrganisationLookUp> GetData();
    }
}
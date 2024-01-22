using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Services
{
    public interface IPoliceDataLookupService
    {
        Task<PublicSectorOrganisationLookUp> GetGbPoliceForces();
    }
}
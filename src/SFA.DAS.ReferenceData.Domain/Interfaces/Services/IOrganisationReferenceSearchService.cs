using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Models.Organisation;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Services
{
    public interface IOrganisationReferenceSearchService
    {
        Task<IEnumerable<Organisation>> Search(string reference);
    }
}

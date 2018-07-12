using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Types;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Services
{
    public interface IEdubaseService
    {
        Task<ICollection<EducationOrganisation>> GetOrganisations();
    }
}

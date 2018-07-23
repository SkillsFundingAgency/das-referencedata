using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Types;
using SFA.DAS.ReferenceData.Types.DTO;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Services
{
    public interface IEdubaseService
    {
        Task<ICollection<EducationOrganisation>> GetOrganisations();
    }
}

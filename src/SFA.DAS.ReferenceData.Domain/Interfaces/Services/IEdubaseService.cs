using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Models.Education;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Services
{
    public interface IEdubaseService
    {
        Task<IEnumerable<EducationOrganisation>> GetOrganisations();
    }
}

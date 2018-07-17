using System.Collections.Generic;
using SFA.DAS.ReferenceData.Types;
using SFA.DAS.ReferenceData.Types.DTO;

namespace SFA.DAS.ReferenceData.Domain.Models.Education
{
    public class EducationalOrganisationLookUp
    {
        public IEnumerable<EducationOrganisation> Organisations { get; set; }
    }
}

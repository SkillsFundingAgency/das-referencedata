using System.Collections.Generic;
using SFA.DAS.ReferenceData.Types;

namespace SFA.DAS.ReferenceData.Domain.Models.Education
{
    public class EducationalOrganisationLookUp
    {
        public IEnumerable<EducationOrganisation> Organisations { get; set; }
    }
}

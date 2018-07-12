using System.Collections.Generic;
using SFA.DAS.ReferenceData.Api.Client.Dto;

namespace SFA.DAS.ReferenceData.Domain.Models.Education
{
    public class EducationalOrganisationLookUp
    {
        public IEnumerable<EducationOrganisation> Organisations { get; set; }
    }
}

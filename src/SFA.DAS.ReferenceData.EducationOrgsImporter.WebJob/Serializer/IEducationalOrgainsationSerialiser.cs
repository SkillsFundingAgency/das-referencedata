using System.Collections.Generic;
using SFA.DAS.ReferenceData.Domain.Models.Education;

namespace SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.Serializer
{
    public interface IEducationalOrgainsationSerialiser
    {
        byte[] SerialiseToJson(IEnumerable<EducationOrganisation> organisations);
    }
}
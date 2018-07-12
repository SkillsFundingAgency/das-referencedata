using System.Collections.Generic;
using SFA.DAS.ReferenceData.Types;

namespace SFA.DAS.ReferenceData.Domain.Models
{
    public class PublicSectorOrganisationLookUp
    {
        public IEnumerable<PublicSectorOrganisation> Organisations { get; set; }
    }
}

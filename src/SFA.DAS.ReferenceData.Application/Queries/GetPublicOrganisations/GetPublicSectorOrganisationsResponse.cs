using System.Collections.Generic;
using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.Application.Queries.GetPublicOrganisations
{
    public class GetPublicSectorOrganisationsResponse
    {
        public ICollection<PublicSectorOrganisation> Organisations { get; set; }
    }
}

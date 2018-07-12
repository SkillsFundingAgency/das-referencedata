using System.Collections.Generic;
using SFA.DAS.ReferenceData.Api.Client.Dto;
using SFA.DAS.ReferenceData.Types;

namespace SFA.DAS.ReferenceData.Application.Queries.SearchOrganisations
{
    public class SearchOrganisationsResponse
    {
        public IEnumerable<Organisation> Organisations { get; set; }
    }
}

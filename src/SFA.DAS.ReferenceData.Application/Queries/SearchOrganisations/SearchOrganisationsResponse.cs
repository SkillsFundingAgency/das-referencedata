using System.Collections.Generic;
using SFA.DAS.ReferenceData.Api.Client.Dto;
using SFA.DAS.ReferenceData.Domain.Models.Organisation;

namespace SFA.DAS.ReferenceData.Application.Queries.SearchOrganisations
{
    public class SearchOrganisationsResponse
    {
        public IEnumerable<Organisation> Organisations { get; set; }
    }
}

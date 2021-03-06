﻿using System.Collections.Generic;
using SFA.DAS.ReferenceData.Types.DTO;

namespace SFA.DAS.ReferenceData.Application.Queries.SearchOrganisations
{
    public class SearchOrganisationsResponse
    {
        public IEnumerable<Organisation> Organisations { get; set; }
    }
}

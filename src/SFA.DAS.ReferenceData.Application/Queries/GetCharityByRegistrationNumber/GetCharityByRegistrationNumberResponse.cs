using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Models.Charity;
using SFA.DAS.ReferenceData.Types;

namespace SFA.DAS.ReferenceData.Application.Queries.GetCharityByRegistrationNumber
{
    public class GetCharityByRegistrationNumberResponse
    {
        public Charity Charity { get; set; }
    }
}

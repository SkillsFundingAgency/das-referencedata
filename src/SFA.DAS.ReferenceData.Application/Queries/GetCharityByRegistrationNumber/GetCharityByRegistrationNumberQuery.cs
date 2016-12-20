using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ReferenceData.Application.Queries.GetCharityByRegistrationNumber
{
    public class GetCharityByRegistrationNumberQuery : IAsyncRequest<GetCharityByRegistrationNumberResponse>
    {
        public int RegistrationNumber { get; set; }
    }
}

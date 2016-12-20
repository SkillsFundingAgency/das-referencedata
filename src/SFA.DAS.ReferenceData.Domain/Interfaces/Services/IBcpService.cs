using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Models.Bcp;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Services
{
    public interface IBcpService
    {
        void ExecuteBcp(BcpRequest request);
    }
}

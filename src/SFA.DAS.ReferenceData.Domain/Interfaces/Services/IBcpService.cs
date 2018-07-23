using SFA.DAS.ReferenceData.Domain.Models.Bcp;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Services
{
    public interface IBcpService
    {
        void ExecuteBcp(BcpRequest request);
    }
}

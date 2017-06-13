using Dfe.Edubase2.SoapApi.Client;

namespace SFA.DAS.ReferenceData.Infrastructure.Factories
{
    public interface IEdubaseClientFactory
    {
        IEstablishmentClient Create();
    }
}

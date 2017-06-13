using Dfe.Edubase2.SoapApi.Client;
using SFA.DAS.ReferenceData.Domain.Configuration;

namespace SFA.DAS.ReferenceData.Infrastructure.Factories
{
    public class EdubaseClientFactory : IEdubaseClientFactory
    {
        private readonly ReferenceDataApiConfiguration _configuration;

        public EdubaseClientFactory(ReferenceDataApiConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEstablishmentClient Create()
        {
            return new EstablishmentClient(_configuration.EdubaseUsername, _configuration.EdubasePassword);
        }
    }
}

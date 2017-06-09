using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.Edubase2.SoapApi.Client.EdubaseService;
using NLog;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models.Education;
using SFA.DAS.ReferenceData.Infrastructure.Factories;

namespace SFA.DAS.ReferenceData.Infrastructure.Services
{
    public class EdubaseService : IEdubaseService
    {
        private readonly IEdubaseClientFactory _factory;
        private readonly ILogger _logger;

        public EdubaseService(IEdubaseClientFactory factory, ILogger logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public async Task<ICollection<EducationOrganisation>> GetOrganisations()
        {
            var client = _factory.Create();

            var filter = new EstablishmentFilter
            {
                Fields = new StringList { "EstablishmentName"}
            };

            try
            {
                var establishments = await client.FindEstablishmentsAsync(filter);

                if (!establishments.Any())
                    return new EducationOrganisation[0];

                return establishments.Select(x => new EducationOrganisation
                {
                    Name = x.EstablishmentName
                }).ToArray();
            }
            catch (Exception e)
            {
                _logger.Error(e, "Could not get organisations from Edubase API");
            }

            return new EducationOrganisation[0];
        }
    }
}

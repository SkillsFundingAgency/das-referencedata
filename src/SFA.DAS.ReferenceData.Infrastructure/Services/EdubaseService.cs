using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.Edubase2.SoapApi.Client.EdubaseService;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Infrastructure.Factories;
using SFA.DAS.ReferenceData.Types.DTO;

namespace SFA.DAS.ReferenceData.Infrastructure.Services
{
    public class EdubaseService : IEdubaseService
    {
        private readonly IEdubaseClientFactory _factory;
        private readonly ILog _logger;

        public EdubaseService(IEdubaseClientFactory factory, ILog logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public async Task<ICollection<EducationOrganisation>> GetOrganisations()
        {
            var client = _factory.Create();

            var filter = new EstablishmentFilter
            {
                Fields = new StringList { "EstablishmentName", "TypeOfEstablishment", "Street", "Locality", "Address3", "Town", "County", "Postcode" }
            };

            try
            {
                var establishments = await client.FindEstablishmentsAsync(filter);

                if (!establishments.Any())
                    return new EducationOrganisation[0];

                return establishments.Select(x => new EducationOrganisation
                {
                    Name = x.EstablishmentName,
                    EducationalType = x.TypeOfEstablishment?.DisplayName ?? string.Empty,
                    AddressLine1 = x.Street,
                    AddressLine2 = x.Locality,
                    AddressLine3 = x.Address3,
                    Town = x.Town,
                    County = x.County?.DisplayName ?? string.Empty,
                    PostCode = x.Postcode
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

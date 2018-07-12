using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Api.Client.Dto;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models.Organisation;

namespace SFA.DAS.ReferenceData.Application.Services.OrganisationSearch
{
    public class PublicSectorSearchService : IOrganisationTextSearchService
    {
        private readonly IPublicSectorOrganisationRepository _repository;

        public PublicSectorSearchService(IPublicSectorOrganisationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Organisation>> Search(string searchTerm, int maximumRecords)
        {
            var publicSectorOrganisations = await _repository.FindOrganisations(searchTerm, maximumRecords, 1);
            return ConvertToOrganisations(publicSectorOrganisations.Data);
        }

        private static IEnumerable<Organisation> ConvertToOrganisations(ICollection<PublicSectorOrganisation> publicSectorOrganisations)
        {
            return publicSectorOrganisations.Select(ConvertToOrganisation);
        }

        private static Organisation ConvertToOrganisation(PublicSectorOrganisation publicSectorOrganisation)
        {
            return new Organisation
            {
                Address = new Address
                {
                    Line1 = publicSectorOrganisation.AddressLine1,
                    Line2 = publicSectorOrganisation.AddressLine2,
                    Line3 = publicSectorOrganisation.AddressLine3,
                    Line4 = publicSectorOrganisation.AddressLine4,
                    Line5 = publicSectorOrganisation.AddressLine5,
                    Postcode = publicSectorOrganisation.PostCode
                },
                Name = publicSectorOrganisation.Name,
                Sector = publicSectorOrganisation.Sector,
                Code = publicSectorOrganisation.OrganisationCode,
                RegistrationDate = null,
                Type = OrganisationType.PublicSector,
                SubType = (OrganisationSubType)Enum.Parse(typeof(OrganisationSubType), publicSectorOrganisation.Source.ToString())
            };
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models.Charity;
using SFA.DAS.ReferenceData.Domain.Models.Organisation;

namespace SFA.DAS.ReferenceData.Application.Services.OrganisationSearch
{
    public class CharitiesSearchService : IOrganisationReferenceSearchService, IOrganisationTextSearchService
    {
        private readonly ICharityRepository _repository;

        public CharitiesSearchService(ICharityRepository repository)
        {
            _repository = repository;
        }

        public async Task<Organisation> Search(string reference)
        {
            if (!IsSearchTermAReference(reference))
            {
                return null;
            }

            var charityNumber = int.Parse(reference);
            var charity = await _repository.GetCharityByRegistrationNumber(charityNumber);
            return ConvertToOrganisation(charity);
        }

        public bool IsSearchTermAReference(string searchTerm)
        {
            int charityNumber;
            return int.TryParse(searchTerm, out charityNumber);
        }

        public async Task<IEnumerable<Organisation>> Search(string searchTerm, int maximumRecords)
        {
            var charities = await _repository.FindCharities(searchTerm, maximumRecords);
            return charities.Select(ConvertToOrganisation);
        }

        private static Organisation ConvertToOrganisation(Charity publicSectorOrganisation)
        {
            return new Organisation
            {
                Address = new Address
                {
                    Line1 = publicSectorOrganisation.Address1,
                    Line2 = publicSectorOrganisation.Address2,
                    Line3 = publicSectorOrganisation.Address3,
                    Line4 = publicSectorOrganisation.Address4,
                    Line5 = publicSectorOrganisation.Address5,
                    Postcode = publicSectorOrganisation.PostCode
                },
                Name = publicSectorOrganisation.Name,
                Sector = null,
                Code = publicSectorOrganisation.RegistrationNumber.ToString(),
                RegistrationDate = publicSectorOrganisation.RegistrationDate,
                Type = OrganisationType.Charity,
                SubType = OrganisationSubType.None
            };
        }
    }
}

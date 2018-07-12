using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Types;


namespace SFA.DAS.ReferenceData.Application.Services.OrganisationSearch
{
    public class EducationalOrganisationSearchService : IOrganisationTextSearchService
    {
        private readonly IEducationalOrganisationRepository _repository;

        public EducationalOrganisationSearchService(IEducationalOrganisationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Organisation>> Search(string searchTerm, int maximumRecords)
        {
            var educationalOrganisations = await _repository.FindOrganisations(searchTerm, maximumRecords, 1);
            return ConvertToOrganisations(educationalOrganisations.Data);
        }

        private static IEnumerable<Organisation> ConvertToOrganisations(IEnumerable<EducationOrganisation> educationalOrganisations)
        {
            return educationalOrganisations.Select(ConvertToOrganisation);
        }

        private static Organisation ConvertToOrganisation(EducationOrganisation educationOrganisation)
        {
            return new Organisation
            {
                Address = new Address
                {
                    Line1 = educationOrganisation.AddressLine1,
                    Line2 = educationOrganisation.AddressLine2,
                    Line3 = educationOrganisation.AddressLine3,
                    Line4 = educationOrganisation.Town,
                    Line5 = educationOrganisation.County,
                    Postcode = educationOrganisation.PostCode
                },
                Name = educationOrganisation.Name,
                Sector = educationOrganisation.EducationalType,
                Code = null,
                RegistrationDate = null,
                Type = OrganisationType.EducationOrganisation,
                SubType = OrganisationSubType.None
            };
        }
    }
}

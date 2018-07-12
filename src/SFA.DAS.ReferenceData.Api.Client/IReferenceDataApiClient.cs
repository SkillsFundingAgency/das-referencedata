using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Api.Client.Dto;
using SFA.DAS.ReferenceData.Types;
using SFA.DAS.ReferenceData.Types.Exceptions;

namespace SFA.DAS.ReferenceData.Api.Client
{
    public interface IReferenceDataApiClient
    {
        Task<Charity> GetCharity(int registrationNumber);

        Task<PagedApiResponse<PublicSectorOrganisation>> SearchPublicSectorOrganisation(string searchTerm, int pageNumber, int pageSize);

        Task<IEnumerable<Organisation>> SearchOrganisations(string searchTerm, int maximumResults = 500);

        Task<PagedApiResponse<EducationOrganisation>> SearchEducationalOrganisation(string searchTerm, int pageNumber, int pageSize);

        /// <summary>
        ///     Returns the latest available details for the supplied organisation.
        /// </summary>
        /// <param name="organisationType">The type of organisation being searched</param>
        /// <param name="identifier">The identifier for the required organisation</param>
        /// <returns>
        ///     Details of the supplied organisation. If the relevant repository (e.g. Companies House) does not contain the 
        ///     specified identifier then an <see cref="OrganisationNotFoundExeption"/> exception will be thrown. 
        /// </returns>
        Task<Organisation> GetLatestDetails(OrganisationType organisationType, string identifier);
    }
}

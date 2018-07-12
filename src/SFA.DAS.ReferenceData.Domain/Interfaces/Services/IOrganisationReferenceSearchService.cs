using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Api.Client.Dto;
using SFA.DAS.ReferenceData.Domain.Models.Organisation;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Services
{
    public interface IOrganisationReferenceSearchService
    {
        OrganisationType OrganisationType { get; }
        bool IsSearchTermAReference(string searchTerm);
        Task<Organisation> Search(string reference);
    }
}

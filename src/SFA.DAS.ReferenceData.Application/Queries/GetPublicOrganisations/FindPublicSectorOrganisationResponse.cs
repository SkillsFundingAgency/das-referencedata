using SFA.DAS.ReferenceData.Api.Client.Dto;
using SFA.DAS.ReferenceData.Types.DTO;

namespace SFA.DAS.ReferenceData.Application.Queries.GetPublicOrganisations
{
    public class FindPublicSectorOrganisationResponse
    {
        public PagedApiResponse<PublicSectorOrganisation> Organisations { get; set; }
    }
}

using SFA.DAS.ReferenceData.Api.Client.Dto;
using SFA.DAS.ReferenceData.Types;

namespace SFA.DAS.ReferenceData.Application.Queries.GetEducationalOrganisations
{
    public class FindEducationalOrganisationsResponse
    {
        public PagedApiResponse<EducationOrganisation> Organisations { get; set; }
    }
}

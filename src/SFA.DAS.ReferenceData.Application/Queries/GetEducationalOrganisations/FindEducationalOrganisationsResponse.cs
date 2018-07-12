using SFA.DAS.ReferenceData.Api.Client.Dto;

namespace SFA.DAS.ReferenceData.Application.Queries.GetEducationalOrganisations
{
    public class FindEducationalOrganisationsResponse
    {
        public PagedApiResponse<EducationOrganisation> Organisations { get; set; }
    }
}

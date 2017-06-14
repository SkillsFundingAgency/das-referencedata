using SFA.DAS.ReferenceData.Api.Client.Dto;
using EducationOrganisation = SFA.DAS.ReferenceData.Domain.Models.Education.EducationOrganisation;

namespace SFA.DAS.ReferenceData.Application.Queries.GetEducationalOrganisations
{
    public class FindEducationalOrganisationsResponse
    {
        public PagedApiResponse<EducationOrganisation> Organisations { get; set; }
    }
}

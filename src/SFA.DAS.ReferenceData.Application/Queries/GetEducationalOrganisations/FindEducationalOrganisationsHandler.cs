using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ReferenceData.Api.Client.Dto;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Types;

namespace SFA.DAS.ReferenceData.Application.Queries.GetEducationalOrganisations
{
    public class FindEducationalOrganisationsHandler : IAsyncRequestHandler<FindEducationalOrganisationsQuery, FindEducationalOrganisationsResponse>
    {
        private readonly IEducationalOrganisationRepository _repository;
        
        public FindEducationalOrganisationsHandler(IEducationalOrganisationRepository repository)
        {
            _repository = repository;
        }
      
        public async Task<FindEducationalOrganisationsResponse> Handle(FindEducationalOrganisationsQuery query)
        {
            var result = await _repository.FindOrganisations(
                query.SearchTerm,
                query.PageSize,
                query.PageNumber);

            return new FindEducationalOrganisationsResponse
            {
                Organisations = new PagedApiResponse<EducationOrganisation>
                {
                    Data = result.Data,
                    PageNumber = result.Page,
                    TotalPages = result.TotalPages
                }
            };
        }
    }
}

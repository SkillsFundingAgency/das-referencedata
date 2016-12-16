using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ReferenceData.Application.Queries.GetPublicOrganisations;
using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.Api.Orchestrators
{
    public class OrganisationOrchestrator
    {
        private readonly IMediator _mediator;

        public OrganisationOrchestrator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<OrchestratorResponse<IEnumerable<PublicSectorOrganisation>>> GetPublicSectorOrganisations()
        {
            var response = await _mediator.SendAsync(new GetPublicSectorOrgainsationsQuery());

            return new OrchestratorResponse<IEnumerable<PublicSectorOrganisation>>
            {
                Data = response.Organisations
            };
        }
    }
}
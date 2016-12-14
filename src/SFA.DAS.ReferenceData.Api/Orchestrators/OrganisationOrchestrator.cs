using System;
using System.Threading.Tasks;
using MediatR;
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

        public async Task<OrchestratorResponse<PublicSectorOrganisation>> GetPublicSectorOrganisations()
        {
            throw new NotImplementedException();
        }
    }
}
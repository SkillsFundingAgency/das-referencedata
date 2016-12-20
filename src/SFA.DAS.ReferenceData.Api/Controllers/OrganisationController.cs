using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using SFA.DAS.ReferenceData.Application.Queries.GetCharityByRegistrationNumber;
using SFA.DAS.ReferenceData.Application.Queries.GetPublicOrganisations;

namespace SFA.DAS.ReferenceData.Api.Controllers
{
    [RoutePrefix("api/organisation")]
    public class OrganisationController : ApiController
    {
        private readonly IMediator _mediator;
       
        public OrganisationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("public", Name = "Public Sector")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPublicSectorOrganisations()
        {
            var response = await _mediator.SendAsync(new GetPublicSectorOrgainsationsQuery());

            return Ok(response.Organisations);
        }

        [Route("charity/{registrationNumber}", Name = "Charity")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCharity(int registrationNumber)
        {
            var query = new GetCharityByRegistrationNumberQuery
            {
                RegistrationNumber = registrationNumber
            };

            var response = await _mediator.SendAsync(query);

            if (response.Charity == null)
            {
                return NotFound();
            }

            return Ok(response.Charity);
        }
    }
}

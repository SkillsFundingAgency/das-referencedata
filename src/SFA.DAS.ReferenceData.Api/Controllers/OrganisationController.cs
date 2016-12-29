using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using SFA.DAS.ReferenceData.Application.Queries.GetCharityByRegistrationNumber;
using SFA.DAS.ReferenceData.Application.Queries.GetPublicOrganisations;

namespace SFA.DAS.ReferenceData.Api.Controllers
{
    [RoutePrefix("api/organisations")]
    public class OrganisationController : ApiController
    {
        private readonly IMediator _mediator;
       
        public OrganisationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("publicsectorbodies", Name = "Public Sector")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> GetPublicSectorOrganisations(string searchTerm = "", int pageSize = 1000, int pageNumber = 1)
        {
            var response = await _mediator.SendAsync(new FindPublicSectorOrgainsationQuery
            {
                SearchTerm = searchTerm,
                PageSize = pageSize,
                PageNumber = pageNumber
            });

            return Ok(response.Organisations);
        }

        [Route("charities/{registrationNumber}", Name = "Charity")]
        [HttpGet]
        [Authorize]
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

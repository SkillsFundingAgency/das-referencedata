using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using SFA.DAS.ReferenceData.Api.Orchestrators;

namespace SFA.DAS.ReferenceData.Api.Controllers
{
    [RoutePrefix("api/organisation")]
    public class OrganisationController : ApiController
    {
        private readonly OrganisationOrchestrator _orchestrator;

        public OrganisationController(OrganisationOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        [Route("/public", Name = "Public Sector")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPublicSectorOrganisations()
        {
            var result = await _orchestrator.GetPublicSectorOrganisations();

            if (result.Status == HttpStatusCode.OK)
            {
                return Ok(result.Data);
            }
            else
            {
                //TODO: Handle unhappy paths.
                return Conflict();
            }
        }
    }
}

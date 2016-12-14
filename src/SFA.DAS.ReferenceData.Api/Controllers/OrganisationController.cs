using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using SFA.DAS.ReferenceData.Api.Orchestrators;

namespace SFA.DAS.ReferenceData.Api.Controllers
{
    [System.Web.Mvc.RoutePrefix("api/organisation")]
    public class OrganisationController : ApiController
    {
        private readonly OrganisationOrchestrator _orchestrator;

        public OrganisationController(OrganisationOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        [System.Web.Mvc.Route("", Name = "PublicSectorOrganisations")]
        [System.Web.Mvc.HttpGet]
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
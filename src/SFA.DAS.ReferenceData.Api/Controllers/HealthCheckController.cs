using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SFA.DAS.ReferenceData.Api.Controllers
{
    public class HealthCheckController : ApiController
    {
        [Route("api/HealthCheck")]
        public IHttpActionResult GetStatus()
        {
            return Ok();
        }
    }
}
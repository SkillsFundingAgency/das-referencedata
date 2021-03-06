﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Api.Attributes;
using SFA.DAS.ReferenceData.Application.Queries.GetCharityByRegistrationNumber;
using SFA.DAS.ReferenceData.Application.Queries.GetEducationalOrganisations;
using SFA.DAS.ReferenceData.Application.Queries.GetIdentifiableOrganisationTypes;
using SFA.DAS.ReferenceData.Application.Queries.GetOrganisation;
using SFA.DAS.ReferenceData.Application.Queries.GetPublicOrganisations;
using SFA.DAS.ReferenceData.Application.Queries.SearchOrganisations;
using SFA.DAS.ReferenceData.Types.DTO;
using SFA.DAS.ReferenceData.Types.Exceptions;

namespace SFA.DAS.ReferenceData.Api.Controllers
{
    [RoutePrefix("api/organisations")]
    public class OrganisationController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly ILog _logger;

        public OrganisationController(IMediator mediator, ILog logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Route("publicsectorbodies", Name = "Public Sector")]
        [HttpGet]
        [ApiAuthorize(Roles = "SearchOrganisations")]
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
        [ApiAuthorize(Roles = "ReadOrganisations")]
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

        [Route("educational", Name = "Educational")]
        [HttpGet]
        [ApiAuthorize(Roles = "ReadOrganisations")]
        public async Task<IHttpActionResult> GetEducaltionalOrganisation(string searchTerm = "", int pageSize = 1000, int pageNumber = 1)
        {
            var query = new FindEducationalOrganisationsQuery
            {
                SearchTerm = searchTerm,
                PageSize = pageSize,
                PageNumber = pageNumber
            };

            var response = await _mediator.SendAsync(query);

            if (response.Organisations == null || !response.Organisations.Data.Any())
            {
                return NotFound();
            }

            return Ok(response.Organisations);
        }

        [Route("", Name = "Search")]
        [HttpGet]
        [ApiAuthorize(Roles = "ReadOrganisations")]
        public async Task<IHttpActionResult> SearchOrganisations(string searchTerm = "", int maximumResults = 500)
        {
            var query = new SearchOrganisationsQuery
            {
                SearchTerm = searchTerm,
                MaximumResults = maximumResults
            };
            SearchOrganisationsResponse response;
            try
            {
                response = await _mediator.SendAsync(query);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Unhandled exception retreiving organisations");
                response = new SearchOrganisationsResponse(){Organisations =  new List<Organisation>()};
            }

            return Ok(response.Organisations);
        }

        [Route("IdentifiableOrganisationTypes")]
        [HttpGet]
        [ApiAuthorize(Roles = "ReadOrganisations")]
        public async Task<IHttpActionResult> GetIdentifiableOrganisationTypes()
        {
            var query = new GetIdentifiableOrganisationTypesQuery();

            var results = await _mediator.SendAsync(query);

            return Ok(results.OrganisationTypes);
        }

        [Route("get")]
        [HttpGet]
        [ApiAuthorize(Roles = "ReadOrganisations")]
        public async Task<IHttpActionResult> Get(string identifier, OrganisationType organisationType)
        {
            var query = new GetOrganisationQuery
            {
                OrganisationType = organisationType,
                Identifier = identifier
            };

            try
            {
                var response = await _mediator.SendAsync(query);
                return Ok(response.Organisation);
            }
            catch (BadOrganisationIdentifierException e)
            {
                return BadRequest(e.Message);
            }
            catch (OperationNotSupportedForOrganisationTypeException e)
            {
                return BadRequest(e.Message);
            }
            catch (OrganisationNotFoundException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}

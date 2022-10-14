// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;
using RockSteadyGo.Core.Api.Models.Matches;
using RockSteadyGo.Core.Api.Models.Matches.Exceptions;
using RockSteadyGo.Core.Api.Services.Foundations.Matches;

namespace RockSteadyGo.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : RESTFulController
    {
        private readonly IMatchService matchService;

        public MatchesController(IMatchService matchService) =>
            this.matchService = matchService;

        [HttpPost]
        public async ValueTask<ActionResult<Match>> PostMatchAsync(Match match)
        {
            try
            {
                Match addedMatch =
                    await this.matchService.AddMatchAsync(match);

                return Created(addedMatch);
            }
            catch (MatchValidationException matchValidationException)
            {
                return BadRequest(matchValidationException.InnerException);
            }
            catch (MatchDependencyValidationException matchValidationException)
                when (matchValidationException.InnerException is InvalidMatchReferenceException)
            {
                return FailedDependency(matchValidationException.InnerException);
            }
            catch (MatchDependencyValidationException matchDependencyValidationException)
               when (matchDependencyValidationException.InnerException is AlreadyExistsMatchException)
            {
                return Conflict(matchDependencyValidationException.InnerException);
            }
            catch (MatchDependencyException matchDependencyException)
            {
                return InternalServerError(matchDependencyException);
            }
            catch (MatchServiceException matchServiceException)
            {
                return InternalServerError(matchServiceException);
            }
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IQueryable<Match>> GetAllMatches()
        {
            try
            {
                IQueryable<Match> retrievedMatches =
                    this.matchService.RetrieveAllMatches();

                return Ok(retrievedMatches);
            }
            catch (MatchDependencyException matchDependencyException)
            {
                return InternalServerError(matchDependencyException);
            }
            catch (MatchServiceException matchServiceException)
            {
                return InternalServerError(matchServiceException);
            }
        }

        [HttpGet("{matchId}")]
        public async ValueTask<ActionResult<Match>> GetMatchByIdAsync(Guid matchId)
        {
            try
            {
                Match match = await this.matchService.RetrieveMatchByIdAsync(matchId);

                return Ok(match);
            }
            catch (MatchValidationException matchValidationException)
                when (matchValidationException.InnerException is NotFoundMatchException)
            {
                return NotFound(matchValidationException.InnerException);
            }
            catch (MatchValidationException matchValidationException)
            {
                return BadRequest(matchValidationException.InnerException);
            }
            catch (MatchDependencyException matchDependencyException)
            {
                return InternalServerError(matchDependencyException);
            }
            catch (MatchServiceException matchServiceException)
            {
                return InternalServerError(matchServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Match>> PutMatchAsync(Match match)
        {
            try
            {
                Match modifiedMatch =
                    await this.matchService.ModifyMatchAsync(match);

                return Ok(modifiedMatch);
            }
            catch (MatchValidationException matchValidationException)
                when (matchValidationException.InnerException is NotFoundMatchException)
            {
                return NotFound(matchValidationException.InnerException);
            }
            catch (MatchValidationException matchValidationException)
            {
                return BadRequest(matchValidationException.InnerException);
            }
            catch (MatchDependencyValidationException matchValidationException)
                when (matchValidationException.InnerException is InvalidMatchReferenceException)
            {
                return FailedDependency(matchValidationException.InnerException);
            }
            catch (MatchDependencyValidationException matchDependencyValidationException)
               when (matchDependencyValidationException.InnerException is AlreadyExistsMatchException)
            {
                return Conflict(matchDependencyValidationException.InnerException);
            }
            catch (MatchDependencyException matchDependencyException)
            {
                return InternalServerError(matchDependencyException);
            }
            catch (MatchServiceException matchServiceException)
            {
                return InternalServerError(matchServiceException);
            }
        }

        [HttpDelete("{matchId}")]
        public async ValueTask<ActionResult<Match>> DeleteMatchByIdAsync(Guid matchId)
        {
            try
            {
                Match deletedMatch =
                    await this.matchService.RemoveMatchByIdAsync(matchId);

                return Ok(deletedMatch);
            }
            catch (MatchValidationException matchValidationException)
                when (matchValidationException.InnerException is NotFoundMatchException)
            {
                return NotFound(matchValidationException.InnerException);
            }
            catch (MatchValidationException matchValidationException)
            {
                return BadRequest(matchValidationException.InnerException);
            }
            catch (MatchDependencyValidationException matchDependencyValidationException)
                when (matchDependencyValidationException.InnerException is LockedMatchException)
            {
                return Locked(matchDependencyValidationException.InnerException);
            }
            catch (MatchDependencyValidationException matchDependencyValidationException)
            {
                return BadRequest(matchDependencyValidationException);
            }
            catch (MatchDependencyException matchDependencyException)
            {
                return InternalServerError(matchDependencyException);
            }
            catch (MatchServiceException matchServiceException)
            {
                return InternalServerError(matchServiceException);
            }
        }
    }
}
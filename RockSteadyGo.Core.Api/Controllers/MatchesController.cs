using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
    }
}
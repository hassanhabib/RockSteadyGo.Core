// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;
using RockSteadyGo.Core.Api.Models.Moves;
using RockSteadyGo.Core.Api.Models.Moves.Exceptions;
using RockSteadyGo.Core.Api.Services.Foundations.Moves;

namespace RockSteadyGo.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovesController : RESTFulController
    {
        private readonly IMoveService moveService;

        public MovesController(IMoveService moveService) =>
            this.moveService = moveService;

        [HttpPost]
        public async ValueTask<ActionResult<Move>> PostMoveAsync(Move move)
        {
            try
            {
                Move addedMove =
                    await this.moveService.AddMoveAsync(move);

                return Created(addedMove);
            }
            catch (MoveValidationException moveValidationException)
            {
                return BadRequest(moveValidationException.InnerException);
            }
            catch (MoveDependencyValidationException moveValidationException)
                when (moveValidationException.InnerException is InvalidMoveReferenceException)
            {
                return FailedDependency(moveValidationException.InnerException);
            }
            catch (MoveDependencyValidationException moveDependencyValidationException)
               when (moveDependencyValidationException.InnerException is AlreadyExistsMoveException)
            {
                return Conflict(moveDependencyValidationException.InnerException);
            }
            catch (MoveDependencyException moveDependencyException)
            {
                return InternalServerError(moveDependencyException);
            }
            catch (MoveServiceException moveServiceException)
            {
                return InternalServerError(moveServiceException);
            }
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IQueryable<Move>> GetAllMoves()
        {
            try
            {
                IQueryable<Move> retrievedMoves =
                    this.moveService.RetrieveAllMoves();

                return Ok(retrievedMoves);
            }
            catch (MoveDependencyException moveDependencyException)
            {
                return InternalServerError(moveDependencyException);
            }
            catch (MoveServiceException moveServiceException)
            {
                return InternalServerError(moveServiceException);
            }
        }
    }
}
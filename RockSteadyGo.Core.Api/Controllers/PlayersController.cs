// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using RockSteadyGo.Core.Api.Models.Players;
using RockSteadyGo.Core.Api.Models.Players.Exceptions;
using RockSteadyGo.Core.Api.Services.Foundations.Players;

namespace RockSteadyGo.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : RESTFulController
    {
        private readonly IPlayerService playerService;

        public PlayersController(IPlayerService playerService) =>
            this.playerService = playerService;

        [HttpPost]
        public async ValueTask<ActionResult<Player>> PostPlayerAsync(Player player)
        {
            try
            {
                Player addedPlayer =
                    await this.playerService.AddPlayerAsync(player);

                return Created(addedPlayer);
            }
            catch (PlayerValidationException playerValidationException)
            {
                return BadRequest(playerValidationException.InnerException);
            }
            catch (PlayerDependencyValidationException playerValidationException)
                when (playerValidationException.InnerException is InvalidPlayerReferenceException)
            {
                return FailedDependency(playerValidationException.InnerException);
            }
            catch (PlayerDependencyValidationException playerDependencyValidationException)
               when (playerDependencyValidationException.InnerException is AlreadyExistsPlayerException)
            {
                return Conflict(playerDependencyValidationException.InnerException);
            }
            catch (PlayerDependencyException playerDependencyException)
            {
                return InternalServerError(playerDependencyException);
            }
            catch (PlayerServiceException playerServiceException)
            {
                return InternalServerError(playerServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Player>> GetAllPlayers()
        {
            try
            {
                IQueryable<Player> retrievedPlayers =
                    this.playerService.RetrieveAllPlayers();

                return Ok(retrievedPlayers);
            }
            catch (PlayerDependencyException playerDependencyException)
            {
                return InternalServerError(playerDependencyException);
            }
            catch (PlayerServiceException playerServiceException)
            {
                return InternalServerError(playerServiceException);
            }
        }

        [HttpGet("{playerId}")]
        public async ValueTask<ActionResult<Player>> GetPlayerByIdAsync(Guid playerId)
        {
            try
            {
                Player player = await this.playerService.RetrievePlayerByIdAsync(playerId);

                return Ok(player);
            }
            catch (PlayerValidationException playerValidationException)
                when (playerValidationException.InnerException is NotFoundPlayerException)
            {
                return NotFound(playerValidationException.InnerException);
            }
            catch (PlayerValidationException playerValidationException)
            {
                return BadRequest(playerValidationException.InnerException);
            }
            catch (PlayerDependencyException playerDependencyException)
            {
                return InternalServerError(playerDependencyException);
            }
            catch (PlayerServiceException playerServiceException)
            {
                return InternalServerError(playerServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Player>> PutPlayerAsync(Player player)
        {
            try
            {
                Player modifiedPlayer =
                    await this.playerService.ModifyPlayerAsync(player);

                return Ok(modifiedPlayer);
            }
            catch (PlayerValidationException playerValidationException)
                when (playerValidationException.InnerException is NotFoundPlayerException)
            {
                return NotFound(playerValidationException.InnerException);
            }
            catch (PlayerValidationException playerValidationException)
            {
                return BadRequest(playerValidationException.InnerException);
            }
            catch (PlayerDependencyValidationException playerValidationException)
                when (playerValidationException.InnerException is InvalidPlayerReferenceException)
            {
                return FailedDependency(playerValidationException.InnerException);
            }
            catch (PlayerDependencyValidationException playerDependencyValidationException)
               when (playerDependencyValidationException.InnerException is AlreadyExistsPlayerException)
            {
                return Conflict(playerDependencyValidationException.InnerException);
            }
            catch (PlayerDependencyException playerDependencyException)
            {
                return InternalServerError(playerDependencyException);
            }
            catch (PlayerServiceException playerServiceException)
            {
                return InternalServerError(playerServiceException);
            }
        }

        [HttpDelete("{playerId}")]
        public async ValueTask<ActionResult<Player>> DeletePlayerByIdAsync(Guid playerId)
        {
            try
            {
                Player deletedPlayer =
                    await this.playerService.RemovePlayerByIdAsync(playerId);

                return Ok(deletedPlayer);
            }
            catch (PlayerValidationException playerValidationException)
                when (playerValidationException.InnerException is NotFoundPlayerException)
            {
                return NotFound(playerValidationException.InnerException);
            }
            catch (PlayerValidationException playerValidationException)
            {
                return BadRequest(playerValidationException.InnerException);
            }
            catch (PlayerDependencyValidationException playerDependencyValidationException)
                when (playerDependencyValidationException.InnerException is LockedPlayerException)
            {
                return Locked(playerDependencyValidationException.InnerException);
            }
            catch (PlayerDependencyValidationException playerDependencyValidationException)
            {
                return BadRequest(playerDependencyValidationException);
            }
            catch (PlayerDependencyException playerDependencyException)
            {
                return InternalServerError(playerDependencyException);
            }
            catch (PlayerServiceException playerServiceException)
            {
                return InternalServerError(playerServiceException);
            }
        }
    }
}
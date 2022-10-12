// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using RockSteadyGo.Core.Api.Models.Players;
using RockSteadyGo.Core.Api.Models.Players.Exceptions;
using Xeptions;

namespace RockSteadyGo.Core.Api.Services.Foundations.Players
{
    public partial class PlayerService
    {
        private delegate ValueTask<Player> ReturningPlayerFunction();

        private async ValueTask<Player> TryCatch(ReturningPlayerFunction returningPlayerFunction)
        {
            try
            {
                return await returningPlayerFunction();
            }
            catch (NullPlayerException nullPlayerException)
            {
                throw CreateAndLogValidationException(nullPlayerException);
            }
            catch (InvalidPlayerException invalidPlayerException)
            {
                throw CreateAndLogValidationException(invalidPlayerException);
            }
            catch (SqlException sqlException)
            {
                var failedPlayerStorageException =
                    new FailedPlayerStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedPlayerStorageException);
            }
        }

        private PlayerValidationException CreateAndLogValidationException(Xeption exception)
        {
            var playerValidationException =
                new PlayerValidationException(exception);

            this.loggingBroker.LogError(playerValidationException);

            return playerValidationException;
        }

        private PlayerDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var playerDependencyException = new PlayerDependencyException(exception);
            this.loggingBroker.LogCritical(playerDependencyException);

            return playerDependencyException;
        }
    }
}
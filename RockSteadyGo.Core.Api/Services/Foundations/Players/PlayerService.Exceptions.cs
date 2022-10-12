// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RockSteadyGo.Core.Api.Models.Players;
using RockSteadyGo.Core.Api.Models.Players.Exceptions;
using Xeptions;

namespace RockSteadyGo.Core.Api.Services.Foundations.Players
{
    public partial class PlayerService
    {
        private delegate ValueTask<Player> ReturningPlayerFunction();
        private delegate IQueryable<Player> ReturningPlayersFunction();

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
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsPlayerException =
                    new AlreadyExistsPlayerException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsPlayerException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidPlayerReferenceException =
                    new InvalidPlayerReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidPlayerReferenceException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedPlayerStorageException =
                    new FailedPlayerStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedPlayerStorageException);
            }
            catch (Exception exception)
            {
                var failedPlayerServiceException =
                    new FailedPlayerServiceException(exception);

                throw CreateAndLogServiceException(failedPlayerServiceException);
            }
        }

        private IQueryable<Player> TryCatch(ReturningPlayersFunction returningPlayersFunction)
        {
            try
            {
                return returningPlayersFunction();
            }
            catch (SqlException sqlException)
            {
                var failedPlayerStorageException =
                    new FailedPlayerStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedPlayerStorageException);
            }
            catch (Exception exception)
            {
                var failedPlayerServiceException =
                    new FailedPlayerServiceException(exception);

                throw CreateAndLogServiceException(failedPlayerServiceException);
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

        private PlayerDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var playerDependencyValidationException =
                new PlayerDependencyValidationException(exception);

            this.loggingBroker.LogError(playerDependencyValidationException);

            return playerDependencyValidationException;
        }

        private PlayerDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var playerDependencyException = new PlayerDependencyException(exception);
            this.loggingBroker.LogError(playerDependencyException);

            return playerDependencyException;
        }

        private PlayerServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var playerServiceException = new PlayerServiceException(exception);
            this.loggingBroker.LogError(playerServiceException);

            return playerServiceException;
        }
    }
}
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
using RockSteadyGo.Core.Api.Models.Moves;
using RockSteadyGo.Core.Api.Models.Moves.Exceptions;
using Xeptions;

namespace RockSteadyGo.Core.Api.Services.Foundations.Moves
{
    public partial class MoveService
    {
        private delegate ValueTask<Move> ReturningMoveFunction();
        private delegate IQueryable<Move> ReturningMovesFunction();

        private async ValueTask<Move> TryCatch(ReturningMoveFunction returningMoveFunction)
        {
            try
            {
                return await returningMoveFunction();
            }
            catch (NullMoveException nullMoveException)
            {
                throw CreateAndLogValidationException(nullMoveException);
            }
            catch (InvalidMoveException invalidMoveException)
            {
                throw CreateAndLogValidationException(invalidMoveException);
            }
            catch (SqlException sqlException)
            {
                var failedMoveStorageException =
                    new FailedMoveStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedMoveStorageException);
            }
            catch (NotFoundMoveException notFoundMoveException)
            {
                throw CreateAndLogValidationException(notFoundMoveException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsMoveException =
                    new AlreadyExistsMoveException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsMoveException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidMoveReferenceException =
                    new InvalidMoveReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidMoveReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedMoveException = new LockedMoveException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedMoveException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedMoveStorageException =
                    new FailedMoveStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedMoveStorageException);
            }
            catch (Exception exception)
            {
                var failedMoveServiceException =
                    new FailedMoveServiceException(exception);

                throw CreateAndLogServiceException(failedMoveServiceException);
            }
        }

        private IQueryable<Move> TryCatch(ReturningMovesFunction returningMovesFunction)
        {
            try
            {
                return returningMovesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedMoveStorageException =
                    new FailedMoveStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedMoveStorageException);
            }
            catch (Exception exception)
            {
                var failedMoveServiceException =
                    new FailedMoveServiceException(exception);

                throw CreateAndLogServiceException(failedMoveServiceException);
            }
        }

        private MoveValidationException CreateAndLogValidationException(Xeption exception)
        {
            var moveValidationException =
                new MoveValidationException(exception);

            this.loggingBroker.LogError(moveValidationException);

            return moveValidationException;
        }

        private MoveDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var moveDependencyException = new MoveDependencyException(exception);
            this.loggingBroker.LogCritical(moveDependencyException);

            return moveDependencyException;
        }

        private MoveDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var moveDependencyValidationException =
                new MoveDependencyValidationException(exception);

            this.loggingBroker.LogError(moveDependencyValidationException);

            return moveDependencyValidationException;
        }

        private MoveDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var moveDependencyException = new MoveDependencyException(exception);
            this.loggingBroker.LogError(moveDependencyException);

            return moveDependencyException;
        }

        private MoveServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var moveServiceException = new MoveServiceException(exception);
            this.loggingBroker.LogError(moveServiceException);

            return moveServiceException;
        }
    }
}
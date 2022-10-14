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
using RockSteadyGo.Core.Api.Models.Matches;
using RockSteadyGo.Core.Api.Models.Matches.Exceptions;
using Xeptions;

namespace RockSteadyGo.Core.Api.Services.Foundations.Matches
{
    public partial class MatchService
    {
        private delegate ValueTask<Match> ReturningMatchFunction();
        private delegate IQueryable<Match> ReturningMatchesFunction();

        private async ValueTask<Match> TryCatch(ReturningMatchFunction returningMatchFunction)
        {
            try
            {
                return await returningMatchFunction();
            }
            catch (NullMatchException nullMatchException)
            {
                throw CreateAndLogValidationException(nullMatchException);
            }
            catch (InvalidMatchException invalidMatchException)
            {
                throw CreateAndLogValidationException(invalidMatchException);
            }
            catch (SqlException sqlException)
            {
                var failedMatchStorageException =
                    new FailedMatchStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedMatchStorageException);
            }
            catch (NotFoundMatchException notFoundMatchException)
            {
                throw CreateAndLogValidationException(notFoundMatchException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsMatchException =
                    new AlreadyExistsMatchException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsMatchException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidMatchReferenceException =
                    new InvalidMatchReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidMatchReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedMatchException = new LockedMatchException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedMatchException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedMatchStorageException =
                    new FailedMatchStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedMatchStorageException);
            }
            catch (Exception exception)
            {
                var failedMatchServiceException =
                    new FailedMatchServiceException(exception);

                throw CreateAndLogServiceException(failedMatchServiceException);
            }
        }

        private IQueryable<Match> TryCatch(ReturningMatchesFunction returningMatchesFunction)
        {
            try
            {
                return returningMatchesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedMatchStorageException =
                    new FailedMatchStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedMatchStorageException);
            }
            catch (Exception exception)
            {
                var failedMatchServiceException =
                    new FailedMatchServiceException(exception);

                throw CreateAndLogServiceException(failedMatchServiceException);
            }
        }

        private MatchValidationException CreateAndLogValidationException(Xeption exception)
        {
            var matchValidationException =
                new MatchValidationException(exception);

            this.loggingBroker.LogError(matchValidationException);

            return matchValidationException;
        }

        private MatchDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var matchDependencyException = new MatchDependencyException(exception);
            this.loggingBroker.LogCritical(matchDependencyException);

            return matchDependencyException;
        }

        private MatchDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var matchDependencyValidationException =
                new MatchDependencyValidationException(exception);

            this.loggingBroker.LogError(matchDependencyValidationException);

            return matchDependencyValidationException;
        }

        private MatchDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var matchDependencyException = new MatchDependencyException(exception);
            this.loggingBroker.LogError(matchDependencyException);

            return matchDependencyException;
        }

        private MatchServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var matchServiceException = new MatchServiceException(exception);
            this.loggingBroker.LogError(matchServiceException);

            return matchServiceException;
        }
    }
}
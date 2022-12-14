// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using RockSteadyGo.Core.Api.Models.Matches.Exceptions;
using Xunit;
using Match = RockSteadyGo.Core.Api.Models.Matches.Match;

namespace RockSteadyGo.Core.Api.Tests.Unit.Services.Foundations.Matches
{
    public partial class MatchServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Match randomMatch = CreateRandomMatch();
            SqlException sqlException = GetSqlException();

            var failedMatchStorageException =
                new FailedMatchStorageException(sqlException);

            var expectedMatchDependencyException =
                new MatchDependencyException(failedMatchStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMatchByIdAsync(randomMatch.Id))
                    .Throws(sqlException);

            // when
            ValueTask<Match> addMatchTask =
                this.matchService.RemoveMatchByIdAsync(randomMatch.Id);

            MatchDependencyException actualMatchDependencyException =
                await Assert.ThrowsAsync<MatchDependencyException>(
                    addMatchTask.AsTask);

            // then
            actualMatchDependencyException.Should()
                .BeEquivalentTo(expectedMatchDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMatchByIdAsync(randomMatch.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedMatchDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteMatchAsync(It.IsAny<Match>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someMatchId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedMatchException =
                new LockedMatchException(databaseUpdateConcurrencyException);

            var expectedMatchDependencyValidationException =
                new MatchDependencyValidationException(lockedMatchException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMatchByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Match> removeMatchByIdTask =
                this.matchService.RemoveMatchByIdAsync(someMatchId);

            MatchDependencyValidationException actualMatchDependencyValidationException =
                await Assert.ThrowsAsync<MatchDependencyValidationException>(
                    removeMatchByIdTask.AsTask);

            // then
            actualMatchDependencyValidationException.Should()
                .BeEquivalentTo(expectedMatchDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMatchByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMatchDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteMatchAsync(It.IsAny<Match>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someMatchId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedMatchStorageException =
                new FailedMatchStorageException(sqlException);

            var expectedMatchDependencyException =
                new MatchDependencyException(failedMatchStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMatchByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Match> deleteMatchTask =
                this.matchService.RemoveMatchByIdAsync(someMatchId);

            MatchDependencyException actualMatchDependencyException =
                await Assert.ThrowsAsync<MatchDependencyException>(
                    deleteMatchTask.AsTask);

            // then
            actualMatchDependencyException.Should()
                .BeEquivalentTo(expectedMatchDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMatchByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedMatchDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someMatchId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedMatchServiceException =
                new FailedMatchServiceException(serviceException);

            var expectedMatchServiceException =
                new MatchServiceException(failedMatchServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMatchByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Match> removeMatchByIdTask =
                this.matchService.RemoveMatchByIdAsync(someMatchId);

            MatchServiceException actualMatchServiceException =
                await Assert.ThrowsAsync<MatchServiceException>(
                    removeMatchByIdTask.AsTask);

            // then
            actualMatchServiceException.Should()
                .BeEquivalentTo(expectedMatchServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMatchByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMatchServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
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
            ValueTask<Match> modifyMatchTask =
                this.matchService.ModifyMatchAsync(randomMatch);

            MatchDependencyException actualMatchDependencyException =
                await Assert.ThrowsAsync<MatchDependencyException>(
                    modifyMatchTask.AsTask);

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
                broker.UpdateMatchAsync(randomMatch),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            Match someMatch = CreateRandomMatch();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidMatchReferenceException =
                new InvalidMatchReferenceException(foreignKeyConstraintConflictException);

            MatchDependencyValidationException expectedMatchDependencyValidationException =
                new MatchDependencyValidationException(invalidMatchReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMatchByIdAsync(someMatch.Id))
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<Match> modifyMatchTask =
                this.matchService.ModifyMatchAsync(someMatch);

            MatchDependencyValidationException actualMatchDependencyValidationException =
                await Assert.ThrowsAsync<MatchDependencyValidationException>(
                    modifyMatchTask.AsTask);

            // then
            actualMatchDependencyValidationException.Should()
                .BeEquivalentTo(expectedMatchDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMatchByIdAsync(someMatch.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedMatchDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateMatchAsync(someMatch),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Match randomMatch = CreateRandomMatch();
            var databaseUpdateException = new DbUpdateException();

            var failedMatchStorageException =
                new FailedMatchStorageException(databaseUpdateException);

            var expectedMatchDependencyException =
                new MatchDependencyException(failedMatchStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMatchByIdAsync(randomMatch.Id))
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Match> modifyMatchTask =
                this.matchService.ModifyMatchAsync(randomMatch);

            MatchDependencyException actualMatchDependencyException =
                await Assert.ThrowsAsync<MatchDependencyException>(
                    modifyMatchTask.AsTask);

            // then
            actualMatchDependencyException.Should()
                .BeEquivalentTo(expectedMatchDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMatchByIdAsync(randomMatch.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMatchDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateMatchAsync(randomMatch),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            Match randomMatch = CreateRandomMatch();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedMatchException =
                new LockedMatchException(databaseUpdateConcurrencyException);

            var expectedMatchDependencyValidationException =
                new MatchDependencyValidationException(lockedMatchException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMatchByIdAsync(randomMatch.Id))
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<Match> modifyMatchTask =
                this.matchService.ModifyMatchAsync(randomMatch);

            MatchDependencyValidationException actualMatchDependencyValidationException =
                await Assert.ThrowsAsync<MatchDependencyValidationException>(
                    modifyMatchTask.AsTask);

            // then
            actualMatchDependencyValidationException.Should()
                .BeEquivalentTo(expectedMatchDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMatchByIdAsync(randomMatch.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMatchDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateMatchAsync(randomMatch),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
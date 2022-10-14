// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
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
    }
}
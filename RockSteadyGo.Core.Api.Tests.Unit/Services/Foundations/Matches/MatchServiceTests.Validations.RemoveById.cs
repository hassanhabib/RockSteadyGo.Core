// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using RockSteadyGo.Core.Api.Models.Matches.Exceptions;
using Xunit;
using Match = RockSteadyGo.Core.Api.Models.Matches.Match;

namespace RockSteadyGo.Core.Api.Tests.Unit.Services.Foundations.Matches
{
    public partial class MatchServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidMatchId = Guid.Empty;

            var invalidMatchException =
                new InvalidMatchException();

            invalidMatchException.AddData(
                key: nameof(Match.Id),
                values: "Id is required");

            var expectedMatchValidationException =
                new MatchValidationException(invalidMatchException);

            // when
            ValueTask<Match> removeMatchByIdTask =
                this.matchService.RemoveMatchByIdAsync(invalidMatchId);

            MatchValidationException actualMatchValidationException =
                await Assert.ThrowsAsync<MatchValidationException>(
                    removeMatchByIdTask.AsTask);

            // then
            actualMatchValidationException.Should()
                .BeEquivalentTo(expectedMatchValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMatchValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteMatchAsync(It.IsAny<Match>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
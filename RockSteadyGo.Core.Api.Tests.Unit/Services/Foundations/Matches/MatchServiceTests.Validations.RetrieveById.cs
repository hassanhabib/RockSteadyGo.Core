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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidMatchId = Guid.Empty;

            var invalidMatchException =
                new InvalidMatchException();

            invalidMatchException.AddData(
                key: nameof(Match.Id),
                values: "Id is required");

            var expectedMatchValidationException =
                new MatchValidationException(invalidMatchException);

            // when
            ValueTask<Match> retrieveMatchByIdTask =
                this.matchService.RetrieveMatchByIdAsync(invalidMatchId);

            MatchValidationException actualMatchValidationException =
                await Assert.ThrowsAsync<MatchValidationException>(
                    retrieveMatchByIdTask.AsTask);

            // then
            actualMatchValidationException.Should()
                .BeEquivalentTo(expectedMatchValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMatchValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMatchByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfMatchIsNotFoundAndLogItAsync()
        {
            //given
            Guid someMatchId = Guid.NewGuid();
            Match noMatch = null;

            var notFoundMatchException =
                new NotFoundMatchException(someMatchId);

            var expectedMatchValidationException =
                new MatchValidationException(notFoundMatchException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMatchByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noMatch);

            //when
            ValueTask<Match> retrieveMatchByIdTask =
                this.matchService.RetrieveMatchByIdAsync(someMatchId);

            MatchValidationException actualMatchValidationException =
                await Assert.ThrowsAsync<MatchValidationException>(
                    retrieveMatchByIdTask.AsTask);

            //then
            actualMatchValidationException.Should().BeEquivalentTo(expectedMatchValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMatchByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMatchValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
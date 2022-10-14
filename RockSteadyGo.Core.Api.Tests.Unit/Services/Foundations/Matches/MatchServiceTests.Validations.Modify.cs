// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using RockSteadyGo.Core.Api.Models.Matches.Exceptions;
using Xunit;
using Match = RockSteadyGo.Core.Api.Models.Matches.Match;

namespace RockSteadyGo.Core.Api.Tests.Unit.Services.Foundations.Matches
{
    public partial class MatchServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfMatchIsNullAndLogItAsync()
        {
            // given
            Match nullMatch = null;
            var nullMatchException = new NullMatchException();

            var expectedMatchValidationException =
                new MatchValidationException(nullMatchException);

            // when
            ValueTask<Match> modifyMatchTask =
                this.matchService.ModifyMatchAsync(nullMatch);

            MatchValidationException actualMatchValidationException =
                await Assert.ThrowsAsync<MatchValidationException>(
                    modifyMatchTask.AsTask);

            // then
            actualMatchValidationException.Should()
                .BeEquivalentTo(expectedMatchValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMatchValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateMatchAsync(It.IsAny<Match>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfMatchIsInvalidAndLogItAsync()
        {
            // given 
            var invalidMatch = new Match();

            var invalidMatchException = new InvalidMatchException();

            invalidMatchException.AddData(
                key: nameof(Match.Id),
                values: "Id is required");

            invalidMatchException.AddData(
                key: nameof(Match.CreatedDate),
                values: "Date is required");

            var expectedMatchValidationException =
                new MatchValidationException(invalidMatchException);

            // when
            ValueTask<Match> modifyMatchTask =
                this.matchService.ModifyMatchAsync(invalidMatch);

            MatchValidationException actualMatchValidationException =
                await Assert.ThrowsAsync<MatchValidationException>(
                    modifyMatchTask.AsTask);

            //then
            actualMatchValidationException.Should()
                .BeEquivalentTo(expectedMatchValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMatchValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateMatchAsync(It.IsAny<Match>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfMatchDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Match randomMatch = CreateRandomMatch(randomDateTimeOffset);
            Match nonExistMatch = randomMatch;
            Match nullMatch = null;

            var notFoundMatchException =
                new NotFoundMatchException(nonExistMatch.Id);

            var expectedMatchValidationException =
                new MatchValidationException(notFoundMatchException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMatchByIdAsync(nonExistMatch.Id))
                .ReturnsAsync(nullMatch);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when 
            ValueTask<Match> modifyMatchTask =
                this.matchService.ModifyMatchAsync(nonExistMatch);

            MatchValidationException actualMatchValidationException =
                await Assert.ThrowsAsync<MatchValidationException>(
                    modifyMatchTask.AsTask);

            // then
            actualMatchValidationException.Should()
                .BeEquivalentTo(expectedMatchValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMatchByIdAsync(nonExistMatch.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMatchValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Match randomMatch = CreateRandomMatch(randomDateTimeOffset);
            Match invalidMatch = randomMatch.DeepClone();
            Match storageMatch = invalidMatch.DeepClone();
            storageMatch.CreatedDate = storageMatch.CreatedDate.AddMinutes(randomMinutes);
            var invalidMatchException = new InvalidMatchException();

            invalidMatchException.AddData(
                key: nameof(Match.CreatedDate),
                values: $"Date is not the same as {nameof(Match.CreatedDate)}");

            var expectedMatchValidationException =
                new MatchValidationException(invalidMatchException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMatchByIdAsync(invalidMatch.Id))
                .ReturnsAsync(storageMatch);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<Match> modifyMatchTask =
                this.matchService.ModifyMatchAsync(invalidMatch);

            MatchValidationException actualMatchValidationException =
                await Assert.ThrowsAsync<MatchValidationException>(
                    modifyMatchTask.AsTask);

            // then
            actualMatchValidationException.Should()
                .BeEquivalentTo(expectedMatchValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMatchByIdAsync(invalidMatch.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedMatchValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using RockSteadyGo.Core.Api.Models.Matches.Exceptions;
using RockSteadyGo.Core.Api.Models.Moves;
using RockSteadyGo.Core.Api.Models.Moves.Exceptions;
using Xunit;

namespace RockSteadyGo.Core.Api.Tests.Unit.Services.Foundations.Moves
{
    public partial class MoveServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfMoveIsNullAndLogItAsync()
        {
            // given
            Move nullMove = null;

            var nullMoveException =
                new NullMoveException();

            var expectedMoveValidationException =
                new MoveValidationException(nullMoveException);

            // when
            ValueTask<Move> addMoveTask =
                this.moveService.AddMoveAsync(nullMove);

            MoveValidationException actualMoveValidationException =
                await Assert.ThrowsAsync<MoveValidationException>(() =>
                    addMoveTask.AsTask());

            // then
            actualMoveValidationException.Should()
                .BeEquivalentTo(expectedMoveValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMoveValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(3)]
        public async Task ShouldThrowValidationExceptionOnAddIfMoveIsInvalidAndLogItAsync(int invalidLocation)
        {
            // given
            var invalidMove = new Move();
            invalidMove.LocationX = invalidLocation;
            invalidMove.LocationY = invalidLocation;

            var invalidMoveException =
                new InvalidMoveException();

            invalidMoveException.AddData(
                key: nameof(Move.Id),
                values: "Id is required");

            invalidMoveException.AddData(
                key: nameof(Move.MatchId),
                values: "Id is required");

            invalidMoveException.AddData(
                key: nameof(Move.PlayerId),
                values: "Id is required");

            invalidMoveException.AddData(
                key: nameof(Move.LocationX),
                values: "Invalid value");

            invalidMoveException.AddData(
                key: nameof(Move.LocationY),
                values: "Invalid value");

            invalidMoveException.AddData(
                key: nameof(Move.CreatedDate),
                values: "Date is required");

            var expectedMoveValidationException =
                new MoveValidationException(invalidMoveException);

            // when
            ValueTask<Move> addMoveTask =
                this.moveService.AddMoveAsync(invalidMove);

            MoveValidationException actualMoveValidationException =
                await Assert.ThrowsAsync<MoveValidationException>(() =>
                    addMoveTask.AsTask());

            // then
            actualMoveValidationException.Should()
                .BeEquivalentTo(expectedMoveValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMoveValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertMoveAsync(It.IsAny<Move>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            DateTimeOffset invalidDateTime =
                randomDateTimeOffset.AddMinutes(minutesBeforeOrAfter);

            Move randomMove = CreateRandomMove(invalidDateTime);
            Move invalidMove = randomMove;

            var invalidMoveException =
                new InvalidMoveException();

            invalidMoveException.AddData(
                key: nameof(Move.CreatedDate),
                values: "Date is not recent");

            var expectedMoveValidationException =
                new MoveValidationException(invalidMoveException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Move> addMoveTask =
                this.moveService.AddMoveAsync(invalidMove);

            MoveValidationException actualMoveValidationException =
                await Assert.ThrowsAsync<MoveValidationException>(() =>
                    addMoveTask.AsTask());

            // then
            actualMoveValidationException.Should()
                .BeEquivalentTo(expectedMoveValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMoveValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertMoveAsync(It.IsAny<Move>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfMatchDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Match randomMatch = CreateRandomModifyMatch(randomDateTimeOffset);
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
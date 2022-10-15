// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
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

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfMoveIsInvalidAndLogItAsync()
        {
            // given
            var invalidMove = new Move();

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
                values: "Value is required");

            invalidMoveException.AddData(
                key: nameof(Move.LocationY),
                values: "Value is required");

            invalidMoveException.AddData(
                key: nameof(Move.Type),
                values: "Value is required");

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
    }
}
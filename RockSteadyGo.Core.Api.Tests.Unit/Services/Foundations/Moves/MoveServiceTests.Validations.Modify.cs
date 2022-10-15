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
        public async Task ShouldThrowValidationExceptionOnModifyIfMoveIsNullAndLogItAsync()
        {
            // given
            Move nullMove = null;
            var nullMoveException = new NullMoveException();

            var expectedMoveValidationException =
                new MoveValidationException(nullMoveException);

            // when
            ValueTask<Move> modifyMoveTask =
                this.moveService.ModifyMoveAsync(nullMove);

            MoveValidationException actualMoveValidationException =
                await Assert.ThrowsAsync<MoveValidationException>(
                    modifyMoveTask.AsTask);

            // then
            actualMoveValidationException.Should()
                .BeEquivalentTo(expectedMoveValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMoveValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateMoveAsync(It.IsAny<Move>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(3)]
        public async Task ShouldThrowValidationExceptionOnModifyIfMoveIsInvalidAndLogItAsync(int invalidLocation)
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
                values: "Value is required");

            invalidMoveException.AddData(
                key: nameof(Move.LocationY),
                values: "Value is required");

            invalidMoveException.AddData(
                key: nameof(Move.CreatedDate),
                values: "Date is required");

            var expectedMoveValidationException =
                new MoveValidationException(invalidMoveException);

            // when
            ValueTask<Move> modifyMoveTask =
                this.moveService.ModifyMoveAsync(invalidMove);

            MoveValidationException actualMoveValidationException =
                await Assert.ThrowsAsync<MoveValidationException>(
                    modifyMoveTask.AsTask);

            //then
            actualMoveValidationException.Should()
                .BeEquivalentTo(expectedMoveValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMoveValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateMoveAsync(It.IsAny<Move>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
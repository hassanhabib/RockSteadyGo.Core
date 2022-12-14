// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
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
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidMoveId = Guid.Empty;

            var invalidMoveException =
                new InvalidMoveException();

            invalidMoveException.AddData(
                key: nameof(Move.Id),
                values: "Id is required");

            var expectedMoveValidationException =
                new MoveValidationException(invalidMoveException);

            // when
            ValueTask<Move> removeMoveByIdTask =
                this.moveService.RemoveMoveByIdAsync(invalidMoveId);

            MoveValidationException actualMoveValidationException =
                await Assert.ThrowsAsync<MoveValidationException>(
                    removeMoveByIdTask.AsTask);

            // then
            actualMoveValidationException.Should()
                .BeEquivalentTo(expectedMoveValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMoveValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteMoveAsync(It.IsAny<Move>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
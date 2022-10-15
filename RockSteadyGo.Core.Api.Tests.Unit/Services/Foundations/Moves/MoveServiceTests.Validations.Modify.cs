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
    }
}
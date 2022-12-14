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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidMoveId = Guid.Empty;

            var invalidMoveException =
                new InvalidMoveException();

            invalidMoveException.AddData(
                key: nameof(Move.Id),
                values: "Id is required");

            var expectedMoveValidationException =
                new MoveValidationException(invalidMoveException);

            // when
            ValueTask<Move> retrieveMoveByIdTask =
                this.moveService.RetrieveMoveByIdAsync(invalidMoveId);

            MoveValidationException actualMoveValidationException =
                await Assert.ThrowsAsync<MoveValidationException>(
                    retrieveMoveByIdTask.AsTask);

            // then
            actualMoveValidationException.Should()
                .BeEquivalentTo(expectedMoveValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMoveValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMoveByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfMoveIsNotFoundAndLogItAsync()
        {
            //given
            Guid someMoveId = Guid.NewGuid();
            Move noMove = null;

            var notFoundMoveException =
                new NotFoundMoveException(someMoveId);

            var expectedMoveValidationException =
                new MoveValidationException(notFoundMoveException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMoveByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noMove);

            //when
            ValueTask<Move> retrieveMoveByIdTask =
                this.moveService.RetrieveMoveByIdAsync(someMoveId);

            MoveValidationException actualMoveValidationException =
                await Assert.ThrowsAsync<MoveValidationException>(
                    retrieveMoveByIdTask.AsTask);

            //then
            actualMoveValidationException.Should().BeEquivalentTo(expectedMoveValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMoveByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMoveValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
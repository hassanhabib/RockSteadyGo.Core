// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using RockSteadyGo.Core.Api.Models.Players;
using RockSteadyGo.Core.Api.Models.Players.Exceptions;
using Xunit;

namespace RockSteadyGo.Core.Api.Tests.Unit.Services.Foundations.Players
{
    public partial class PlayerServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidPlayerId = Guid.Empty;

            var invalidPlayerException =
                new InvalidPlayerException();

            invalidPlayerException.AddData(
                key: nameof(Player.Id),
                values: "Id is required");

            var expectedPlayerValidationException =
                new PlayerValidationException(invalidPlayerException);

            // when
            ValueTask<Player> removePlayerByIdTask =
                this.playerService.RemovePlayerByIdAsync(invalidPlayerId);

            PlayerValidationException actualPlayerValidationException =
                await Assert.ThrowsAsync<PlayerValidationException>(
                    removePlayerByIdTask.AsTask);

            // then
            actualPlayerValidationException.Should()
                .BeEquivalentTo(expectedPlayerValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePlayerAsync(It.IsAny<Player>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfPlayerDoesNotExistAndLogItAsync()
        {
            //given
            Guid somePlayerId = Guid.NewGuid();
            Player noPlayer = null;

            var notFoundPlayerException =
                new NotFoundPlayerException(somePlayerId);

            var expectedPlayerValidationException =
                new PlayerValidationException(notFoundPlayerException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPlayerByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noPlayer);

            //when
            ValueTask<Player> removePlayerByIdTask =
                this.playerService.RemovePlayerByIdAsync(somePlayerId);

            PlayerValidationException actualPlayerValidationException =
                await Assert.ThrowsAsync<PlayerValidationException>(
                    removePlayerByIdTask.AsTask);

            //then
            actualPlayerValidationException.Should().BeEquivalentTo(expectedPlayerValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPlayerByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
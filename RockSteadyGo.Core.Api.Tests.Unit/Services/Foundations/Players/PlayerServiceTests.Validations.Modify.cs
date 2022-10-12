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
        public async Task ShouldThrowValidationExceptionOnModifyIfPlayerIsNullAndLogItAsync()
        {
            // given
            Player nullPlayer = null;
            var nullPlayerException = new NullPlayerException();

            var expectedPlayerValidationException =
                new PlayerValidationException(nullPlayerException);

            // when
            ValueTask<Player> modifyPlayerTask =
                this.playerService.ModifyPlayerAsync(nullPlayer);

            PlayerValidationException actualPlayerValidationException =
                await Assert.ThrowsAsync<PlayerValidationException>(
                    modifyPlayerTask.AsTask);

            // then
            actualPlayerValidationException.Should()
                .BeEquivalentTo(expectedPlayerValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePlayerAsync(It.IsAny<Player>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfPlayerIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidPlayer = new Player
            {
                Name = invalidText,
                Username = invalidText,
            };

            var invalidPlayerException = new InvalidPlayerException();

            invalidPlayerException.AddData(
                key: nameof(Player.Id),
                values: "Id is required");

            invalidPlayerException.AddData(
                key: nameof(Player.Name),
                values: "Text is required");

            invalidPlayerException.AddData(
                key: nameof(Player.Username),
                values: "Text is required");

            invalidPlayerException.AddData(
                key: nameof(Player.CreatedDate),
                values: "Date is required");

            var expectedPlayerValidationException =
                new PlayerValidationException(invalidPlayerException);

            // when
            ValueTask<Player> modifyPlayerTask =
                this.playerService.ModifyPlayerAsync(invalidPlayer);

            PlayerValidationException actualPlayerValidationException =
                await Assert.ThrowsAsync<PlayerValidationException>(
                    modifyPlayerTask.AsTask);

            //then
            actualPlayerValidationException.Should()
                .BeEquivalentTo(expectedPlayerValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePlayerAsync(It.IsAny<Player>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfPlayerDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Player randomPlayer = CreateRandomModifyPlayer(randomDateTimeOffset);
            Player nonExistPlayer = randomPlayer;
            Player nullPlayer = null;

            var notFoundPlayerException =
                new NotFoundPlayerException(nonExistPlayer.Id);

            var expectedPlayerValidationException =
                new PlayerValidationException(notFoundPlayerException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPlayerByIdAsync(nonExistPlayer.Id))
                .ReturnsAsync(nullPlayer);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when 
            ValueTask<Player> modifyPlayerTask =
                this.playerService.ModifyPlayerAsync(nonExistPlayer);

            PlayerValidationException actualPlayerValidationException =
                await Assert.ThrowsAsync<PlayerValidationException>(
                    modifyPlayerTask.AsTask);

            // then
            actualPlayerValidationException.Should()
                .BeEquivalentTo(expectedPlayerValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPlayerByIdAsync(nonExistPlayer.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
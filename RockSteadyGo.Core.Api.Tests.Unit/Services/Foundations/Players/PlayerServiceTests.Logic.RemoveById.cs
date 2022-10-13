using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using RockSteadyGo.Core.Api.Models.Players;
using Xunit;

namespace RockSteadyGo.Core.Api.Tests.Unit.Services.Foundations.Players
{
    public partial class PlayerServiceTests
    {
        [Fact]
        public async Task ShouldRemovePlayerByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputPlayerId = randomId;
            Player randomPlayer = CreateRandomPlayer();
            Player storagePlayer = randomPlayer;
            Player expectedInputPlayer = storagePlayer;
            Player deletedPlayer = expectedInputPlayer;
            Player expectedPlayer = deletedPlayer.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPlayerByIdAsync(inputPlayerId))
                    .ReturnsAsync(storagePlayer);

            this.storageBrokerMock.Setup(broker =>
                broker.DeletePlayerAsync(expectedInputPlayer))
                    .ReturnsAsync(deletedPlayer);

            // when
            Player actualPlayer = await this.playerService
                .RemovePlayerByIdAsync(inputPlayerId);

            // then
            actualPlayer.Should().BeEquivalentTo(expectedPlayer);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPlayerByIdAsync(inputPlayerId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePlayerAsync(expectedInputPlayer),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
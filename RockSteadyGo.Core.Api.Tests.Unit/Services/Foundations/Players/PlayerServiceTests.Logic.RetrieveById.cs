// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

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
        public async Task ShouldRetrievePlayerByIdAsync()
        {
            // given
            Player randomPlayer = CreateRandomPlayer();
            Player inputPlayer = randomPlayer;
            Player storagePlayer = randomPlayer;
            Player expectedPlayer = storagePlayer.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPlayerByIdAsync(inputPlayer.Id))
                    .ReturnsAsync(storagePlayer);

            // when
            Player actualPlayer =
                await this.playerService.RetrievePlayerByIdAsync(inputPlayer.Id);

            // then
            actualPlayer.Should().BeEquivalentTo(expectedPlayer);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPlayerByIdAsync(inputPlayer.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
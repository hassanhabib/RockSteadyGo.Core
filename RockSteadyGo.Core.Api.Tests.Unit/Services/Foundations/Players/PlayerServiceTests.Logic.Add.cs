// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

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
        public async Task ShouldAddPlayerAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            Player randomPlayer = CreateRandomPlayer(randomDateTimeOffset);
            Player inputPlayer = randomPlayer;
            Player storagePlayer = inputPlayer;
            Player expectedPlayer = storagePlayer.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPlayerAsync(inputPlayer))
                    .ReturnsAsync(storagePlayer);

            // when
            Player actualPlayer = await this.playerService
                .AddPlayerAsync(inputPlayer);

            // then
            actualPlayer.Should().BeEquivalentTo(expectedPlayer);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPlayerAsync(inputPlayer),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

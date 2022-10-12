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
        public async Task ShouldModifyPlayerAsync()
        {
            // given
            int randomDaysInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Player randomPlayer = CreateRandomPlayer(randomDateTimeOffset.AddDays(randomDaysInPast));
            Player storagePlayer = randomPlayer;

            Player randomModifiedPlayer = CreateRandomModifyPlayer(
                id: storagePlayer.Id,
                createdDate: randomDateTimeOffset,
                updatedDate: randomDateTimeOffset);

            Player inputPlayer = randomModifiedPlayer;
            Player modifiedPlayer = inputPlayer;
            Player expectedPlayer = modifiedPlayer.DeepClone();
            Guid playerId = randomPlayer.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPlayerByIdAsync(playerId))
                    .ReturnsAsync(storagePlayer);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdatePlayerAsync(inputPlayer))
                    .ReturnsAsync(modifiedPlayer);

            // when
            Player actualPlayer =
                await this.playerService.ModifyPlayerAsync(inputPlayer);

            // then
            actualPlayer.Should().BeEquivalentTo(expectedPlayer);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPlayerByIdAsync(inputPlayer.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePlayerAsync(inputPlayer),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
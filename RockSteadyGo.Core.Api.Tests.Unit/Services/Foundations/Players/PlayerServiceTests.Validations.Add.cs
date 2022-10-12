// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionOnAddIfPlayerIsNullAndLogItAsync()
        {
            // given
            Player nullPlayer = null;

            var nullPlayerException =
                new NullPlayerException();

            var expectedPlayerValidationException =
                new PlayerValidationException(nullPlayerException);

            // when
            ValueTask<Player> addPlayerTask =
                this.playerService.AddPlayerAsync(nullPlayer);

            PlayerValidationException actualPlayerValidationException =
                await Assert.ThrowsAsync<PlayerValidationException>(() =>
                    addPlayerTask.AsTask());

            // then
            actualPlayerValidationException.Should()
                .BeEquivalentTo(expectedPlayerValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}

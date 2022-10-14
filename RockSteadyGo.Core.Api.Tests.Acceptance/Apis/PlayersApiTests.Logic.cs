using System.Threading.Tasks;
using FluentAssertions;
using RockSteadyGo.Core.Api.Tests.Acceptance.Models.Players;
using Xunit;

namespace RockSteadyGo.Core.Api.Tests.Acceptance.Apis.Players
{
    public partial class PlayersApiTests
    {
        [Fact]
        public async Task ShouldPostPlayerAsync()
        {
            // given
            Player randomPlayer = CreateRandomPlayer();
            Player inputPlayer = randomPlayer;
            Player expectedPlayer = inputPlayer;

            // when 
            await this.apiBroker.PostPlayerAsync(inputPlayer);

            Player actualPlayer =
                await this.apiBroker.GetPlayerByIdAsync(inputPlayer.Id);

            // then
            actualPlayer.Should().BeEquivalentTo(expectedPlayer);
            await this.apiBroker.DeletePlayerByIdAsync(actualPlayer.Id);
        }
    }
}
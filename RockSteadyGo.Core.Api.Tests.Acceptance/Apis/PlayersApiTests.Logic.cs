using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using RESTFulSense.Exceptions;
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

        [Fact]
        public async Task ShouldGetAllPlayersAsync()
        {
            // given
            List<Player> randomPlayers = await PostRandomPlayersAsync();
            List<Player> expectedPlayers = randomPlayers;

            // when
            List<Player> actualPlayers = await this.apiBroker.GetAllPlayersAsync();

            // then
            foreach (Player expectedPlayer in expectedPlayers)
            {
                Player actualPlayer = actualPlayers.Single(approval => approval.Id == expectedPlayer.Id);
                actualPlayer.Should().BeEquivalentTo(expectedPlayer);
                await this.apiBroker.DeletePlayerByIdAsync(actualPlayer.Id);
            }
        }

        [Fact]
        public async Task ShouldGetPlayerAsync()
        {
            // given
            Player randomPlayer = await PostRandomPlayerAsync();
            Player expectedPlayer = randomPlayer;

            // when
            Player actualPlayer = await this.apiBroker.GetPlayerByIdAsync(randomPlayer.Id);

            // then
            actualPlayer.Should().BeEquivalentTo(expectedPlayer);
            await this.apiBroker.DeletePlayerByIdAsync(actualPlayer.Id);
        }

        [Fact]
        public async Task ShouldPutPlayerAsync()
        {
            // given
            Player randomPlayer = await PostRandomPlayerAsync();
            Player modifiedPlayer = UpdatePlayerWithRandomValues(randomPlayer);

            // when
            await this.apiBroker.PutPlayerAsync(modifiedPlayer);
            Player actualPlayer = await this.apiBroker.GetPlayerByIdAsync(randomPlayer.Id);

            // then
            actualPlayer.Should().BeEquivalentTo(modifiedPlayer);
            await this.apiBroker.DeletePlayerByIdAsync(actualPlayer.Id);
        }

        [Fact]
        public async Task ShouldDeletePlayerAsync()
        {
            // given
            Player randomPlayer = await PostRandomPlayerAsync();
            Player inputPlayer = randomPlayer;
            Player expectedPlayer = inputPlayer;

            // when
            Player deletedPlayer =
                await this.apiBroker.DeletePlayerByIdAsync(inputPlayer.Id);

            ValueTask<Player> getPlayerbyIdTask =
                this.apiBroker.GetPlayerByIdAsync(inputPlayer.Id);

            // then
            deletedPlayer.Should().BeEquivalentTo(expectedPlayer);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
                getPlayerbyIdTask.AsTask());
        }
    }
}
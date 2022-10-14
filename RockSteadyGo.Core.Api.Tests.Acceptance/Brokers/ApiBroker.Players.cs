using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RockSteadyGo.Core.Api.Tests.Acceptance.Models.Players;

namespace RockSteadyGo.Core.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string PlayersRelativeUrl = "api/players";

        public async ValueTask<Player> PostPlayerAsync(Player player) =>
            await this.apiFactoryClient.PostContentAsync(PlayersRelativeUrl, player);

        public async ValueTask<Player> GetPlayerByIdAsync(Guid playerId) =>
            await this.apiFactoryClient.GetContentAsync<Player>($"{PlayersRelativeUrl}/{playerId}");

        public async ValueTask<List<Player>> GetAllPlayersAsync() =>
          await this.apiFactoryClient.GetContentAsync<List<Player>>($"{PlayersRelativeUrl}/");

        public async ValueTask<Player> PutPlayerAsync(Player player) =>
            await this.apiFactoryClient.PutContentAsync(PlayersRelativeUrl, player);

        public async ValueTask<Player> DeletePlayerByIdAsync(Guid playerId) =>
            await this.apiFactoryClient.DeleteContentAsync<Player>($"{PlayersRelativeUrl}/{playerId}");
    }
}

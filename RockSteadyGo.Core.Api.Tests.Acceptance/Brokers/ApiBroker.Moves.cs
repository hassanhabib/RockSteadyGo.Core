using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RockSteadyGo.Core.Api.Tests.Acceptance.Models.Moves;

namespace RockSteadyGo.Core.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string MovesRelativeUrl = "api/moves";

        public async ValueTask<Move> PostMoveAsync(Move move) =>
            await this.apiFactoryClient.PostContentAsync(MovesRelativeUrl, move);

        public async ValueTask<Move> GetMoveByIdAsync(Guid moveId) =>
            await this.apiFactoryClient.GetContentAsync<Move>($"{MovesRelativeUrl}/{moveId}");

        public async ValueTask<List<Move>> GetAllMovesAsync() =>
          await this.apiFactoryClient.GetContentAsync<List<Move>>($"{MovesRelativeUrl}/");

        public async ValueTask<Move> PutMoveAsync(Move move) =>
            await this.apiFactoryClient.PutContentAsync(MovesRelativeUrl, move);

        public async ValueTask<Move> DeleteMoveByIdAsync(Guid moveId) =>
            await this.apiFactoryClient.DeleteContentAsync<Move>($"{MovesRelativeUrl}/{moveId}");
    }
}

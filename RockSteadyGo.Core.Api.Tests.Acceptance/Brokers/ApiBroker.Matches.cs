// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RockSteadyGo.Core.Api.Tests.Acceptance.Models.Matches;

namespace RockSteadyGo.Core.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string MatchesRelativeUrl = "api/matches";

        public async ValueTask<Match> PostMatchAsync(Match match) =>
            await this.apiFactoryClient.PostContentAsync(MatchesRelativeUrl, match);

        public async ValueTask<Match> GetMatchByIdAsync(Guid matchId) =>
            await this.apiFactoryClient.GetContentAsync<Match>($"{MatchesRelativeUrl}/{matchId}");

        public async ValueTask<List<Match>> GetAllMatchesAsync() =>
          await this.apiFactoryClient.GetContentAsync<List<Match>>($"{MatchesRelativeUrl}/");

        public async ValueTask<Match> PutMatchAsync(Match match) =>
            await this.apiFactoryClient.PutContentAsync(MatchesRelativeUrl, match);

        public async ValueTask<Match> DeleteMatchByIdAsync(Guid matchId) =>
            await this.apiFactoryClient.DeleteContentAsync<Match>($"{MatchesRelativeUrl}/{matchId}");
    }
}

// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using RockSteadyGo.Core.Api.Tests.Acceptance.Brokers;
using RockSteadyGo.Core.Api.Tests.Acceptance.Models.Matches;
using RockSteadyGo.Core.Api.Tests.Acceptance.Models.Moves;
using RockSteadyGo.Core.Api.Tests.Acceptance.Models.Players;
using Tynamix.ObjectFiller;
using Xunit;

namespace RockSteadyGo.Core.Api.Tests.Acceptance.Apis.Moves
{
    [Collection(nameof(ApiTestCollection))]
    public partial class MovesApiTests
    {
        private readonly ApiBroker apiBroker;

        public MovesApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private async ValueTask<Match> PostRandomMatchAsync()
        {
            Match randomMatch = CreateRandomMatch();
            await this.apiBroker.PostMatchAsync(randomMatch);

            return randomMatch;
        }

        private static Match CreateRandomMatch() =>
            CreateRandomMatchFiller().Create();

        private static Filler<Match> CreateRandomMatchFiller()
        {
            Guid userId = Guid.NewGuid();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<Match>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(match => match.CreatedDate).Use(now);

            return filler;
        }

        private async ValueTask<Player> PostRandomPlayerAsync()
        {
            Player randomPlayer = CreateRandomPlayer();
            await this.apiBroker.PostPlayerAsync(randomPlayer);

            return randomPlayer;
        }

        private static Player CreateRandomPlayer() =>
            CreateRandomPlayerFiller().Create();

        private static Filler<Player> CreateRandomPlayerFiller()
        {
            Guid userId = Guid.NewGuid();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<Player>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(player => player.CreatedDate).Use(now);

            return filler;
        }

        private static Move CreateRandomMove(Guid matchId, Guid playerId) =>
            CreateRandomMoveFiller(matchId, playerId).Create();

        private static Filler<Move> CreateRandomMoveFiller(Guid matchId, Guid playerId)
        {
            Guid userId = Guid.NewGuid();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<Move>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(move => move.MatchId).Use(matchId)
                .OnProperty(move => move.PlayerId).Use(playerId)
                .OnProperty(move => move.CreatedDate).Use(now);

            return filler;
        }
    }
}
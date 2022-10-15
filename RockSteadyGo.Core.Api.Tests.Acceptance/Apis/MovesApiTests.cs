// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
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

        private static int GetRandomPosition() =>
            new IntRange(min: 0, max: 2).GetValue();

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
        private int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private async ValueTask<Move> PostRandomMoveAsync(Guid matchId, Guid playerId)
        {
            Move randomMove = CreateRandomMove(matchId, playerId);
            await this.apiBroker.PostMoveAsync(randomMove);

            return randomMove;
        }

        private async ValueTask<List<Move>> PostRandomMovesAsync(Guid matchId, Guid playerId)
        {
            int randomNumber = GetRandomNumber();
            var randomMoves = new List<Move>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomMoves.Add(await PostRandomMoveAsync(matchId, playerId));
            }

            return randomMoves;
        }

        private static Filler<Move> CreateRandomMoveFiller(Guid matchId, Guid playerId)
        {
            Guid userId = Guid.NewGuid();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<Move>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(move => move.MatchId).Use(matchId)
                .OnProperty(move => move.PlayerId).Use(playerId)
                .OnProperty(move => move.LocationX).Use(GetRandomPosition())
                .OnProperty(move => move.LocationY).Use(GetRandomPosition())
                .OnProperty(move => move.CreatedDate).Use(now);

            return filler;
        }
    }
}
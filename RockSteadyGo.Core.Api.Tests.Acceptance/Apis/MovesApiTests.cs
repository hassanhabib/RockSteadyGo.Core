using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RockSteadyGo.Core.Api.Tests.Acceptance.Brokers;
using RockSteadyGo.Core.Api.Tests.Acceptance.Models.Moves;
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

        private int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Move UpdateMoveWithRandomValues(Move inputMove)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<Move>();

            filler.Setup()
                .OnProperty(move => move.Id).Use(inputMove.Id)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnProperty(move => move.CreatedDate).Use(inputMove.CreatedDate)
                .OnProperty(move => move.CreatedByUserId).Use(inputMove.CreatedByUserId)
                .OnProperty(move => move.UpdatedDate).Use(now);

            return filler.Create();
        }

        private async ValueTask<Move> PostRandomMoveAsync()
        {
            Move randomMove = CreateRandomMove();
            await this.apiBroker.PostMoveAsync(randomMove);

            return randomMove;
        }

        private async ValueTask<List<Move>> PostRandomMovesAsync()
        {
            int randomNumber = GetRandomNumber();
            var randomMoves = new List<Move>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomMoves.Add(await PostRandomMoveAsync());
            }

            return randomMoves;
        }

        private static Move CreateRandomMove() =>
            CreateRandomMoveFiller().Create();

        private static Filler<Move> CreateRandomMoveFiller()
        {
            Guid userId = Guid.NewGuid();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<Move>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(move => move.CreatedDate).Use(now)
                .OnProperty(move => move.CreatedByUserId).Use(userId)
                .OnProperty(move => move.UpdatedDate).Use(now)
                .OnProperty(move => move.UpdatedByUserId).Use(userId);

            return filler;
        }
    }
}
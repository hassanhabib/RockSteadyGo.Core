using System;
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
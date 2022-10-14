using System;
using RockSteadyGo.Core.Api.Tests.Acceptance.Brokers;
using RockSteadyGo.Core.Api.Tests.Acceptance.Models.Players;
using Tynamix.ObjectFiller;
using Xunit;

namespace RockSteadyGo.Core.Api.Tests.Acceptance.Apis.Players
{
    [Collection(nameof(ApiTestCollection))]
    public partial class PlayersApiTests
    {
        private readonly ApiBroker apiBroker;

        public PlayersApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private static Player CreateRandomPlayer() =>
            CreateRandomPlayerFiller().Create();

        private static Filler<Player> CreateRandomPlayerFiller()
        {
            Guid userId = Guid.NewGuid();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<Player>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(player => player.CreatedDate).Use(now)
                .OnProperty(player => player.CreatedByUserId).Use(userId)
                .OnProperty(player => player.UpdatedDate).Use(now)
                .OnProperty(player => player.UpdatedByUserId).Use(userId);

            return filler;
        }
    }
}
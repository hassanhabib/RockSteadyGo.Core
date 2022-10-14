using System;
using RockSteadyGo.Core.Api.Tests.Acceptance.Brokers;
using RockSteadyGo.Core.Api.Tests.Acceptance.Models.Matches;
using Tynamix.ObjectFiller;
using Xunit;

namespace RockSteadyGo.Core.Api.Tests.Acceptance.Apis.Matches
{
    [Collection(nameof(ApiTestCollection))]
    public partial class MatchesApiTests
    {
        private readonly ApiBroker apiBroker;

        public MatchesApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private static Match CreateRandomMatch() =>
            CreateRandomMatchFiller().Create();

        private static Filler<Match> CreateRandomMatchFiller()
        {
            Guid userId = Guid.NewGuid();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<Match>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(match => match.CreatedDate).Use(now)
                .OnProperty(match => match.CreatedByUserId).Use(userId)
                .OnProperty(match => match.UpdatedDate).Use(now)
                .OnProperty(match => match.UpdatedByUserId).Use(userId);

            return filler;
        }
    }
}
// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        private int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private async ValueTask<Match> PostRandomMatchAsync()
        {
            Match randomMatch = CreateRandomMatch();
            await this.apiBroker.PostMatchAsync(randomMatch);

            return randomMatch;
        }

        private async ValueTask<List<Match>> PostRandomMatchesAsync()
        {
            int randomNumber = GetRandomNumber();
            var randomMatches = new List<Match>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomMatches.Add(await PostRandomMatchAsync());
            }

            return randomMatches;
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
    }
}
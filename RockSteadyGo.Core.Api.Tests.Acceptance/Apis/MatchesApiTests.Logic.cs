using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using RockSteadyGo.Core.Api.Tests.Acceptance.Models.Matches;
using Xunit;

namespace RockSteadyGo.Core.Api.Tests.Acceptance.Apis.Matches
{
    public partial class MatchesApiTests
    {
        [Fact]
        public async Task ShouldPostMatchAsync()
        {
            // given
            Match randomMatch = CreateRandomMatch();
            Match inputMatch = randomMatch;
            Match expectedMatch = inputMatch;

            // when 
            await this.apiBroker.PostMatchAsync(inputMatch);

            Match actualMatch =
                await this.apiBroker.GetMatchByIdAsync(inputMatch.Id);

            // then
            actualMatch.Should().BeEquivalentTo(expectedMatch);
            await this.apiBroker.DeleteMatchByIdAsync(actualMatch.Id);
        }

        [Fact]
        public async Task ShouldGetAllMatchesAsync()
        {
            // given
            List<Match> randomMatches = await PostRandomMatchesAsync();
            List<Match> expectedMatches = randomMatches;

            // when
            List<Match> actualMatches = await this.apiBroker.GetAllMatchesAsync();

            // then
            foreach (Match expectedMatch in expectedMatches)
            {
                Match actualMatch = actualMatches.Single(approval => approval.Id == expectedMatch.Id);
                actualMatch.Should().BeEquivalentTo(expectedMatch);
                await this.apiBroker.DeleteMatchByIdAsync(actualMatch.Id);
            }
        }
    }
}
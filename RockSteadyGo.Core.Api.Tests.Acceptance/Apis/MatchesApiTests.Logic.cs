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
    }
}
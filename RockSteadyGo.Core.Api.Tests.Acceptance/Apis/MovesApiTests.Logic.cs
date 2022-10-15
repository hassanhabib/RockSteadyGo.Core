// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using RockSteadyGo.Core.Api.Tests.Acceptance.Models.Matches;
using RockSteadyGo.Core.Api.Tests.Acceptance.Models.Moves;
using RockSteadyGo.Core.Api.Tests.Acceptance.Models.Players;
using Xunit;

namespace RockSteadyGo.Core.Api.Tests.Acceptance.Apis.Moves
{
    public partial class MovesApiTests
    {
        [Fact]
        public async Task ShouldPostMoveAsync()
        {
            // given
            Match randomMatch = await PostRandomMatchAsync();
            Player randomPlayer = await PostRandomPlayerAsync();
            Move randomMove = CreateRandomMove(randomMatch.Id, randomPlayer.Id);
            Move inputMove = randomMove;
            Move expectedMove = inputMove;

            // when 
            await this.apiBroker.PostMoveAsync(inputMove);

            Move actualMove =
                await this.apiBroker.GetMoveByIdAsync(inputMove.Id);

            // then
            actualMove.Should().BeEquivalentTo(expectedMove);
            await this.apiBroker.DeleteMoveByIdAsync(actualMove.Id);
            await this.apiBroker.DeletePlayerByIdAsync(randomPlayer.Id);
            await this.apiBroker.DeleteMatchByIdAsync(randomMatch.Id);
        }
    }
}
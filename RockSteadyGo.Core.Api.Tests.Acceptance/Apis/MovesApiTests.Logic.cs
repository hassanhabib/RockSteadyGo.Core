using System.Threading.Tasks;
using FluentAssertions;
using RockSteadyGo.Core.Api.Tests.Acceptance.Models.Moves;
using Xunit;

namespace RockSteadyGo.Core.Api.Tests.Acceptance.Apis.Moves
{
    public partial class MovesApiTests
    {
        [Fact]
        public async Task ShouldPostMoveAsync()
        {
            // given
            Move randomMove = CreateRandomMove();
            Move inputMove = randomMove;
            Move expectedMove = inputMove;

            // when 
            await this.apiBroker.PostMoveAsync(inputMove);

            Move actualMove =
                await this.apiBroker.GetMoveByIdAsync(inputMove.Id);

            // then
            actualMove.Should().BeEquivalentTo(expectedMove);
            await this.apiBroker.DeleteMoveByIdAsync(actualMove.Id);
        }
    }
}
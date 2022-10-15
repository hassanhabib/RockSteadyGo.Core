using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using RESTFulSense.Exceptions;
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

        [Fact]
        public async Task ShouldGetAllMovesAsync()
        {
            // given
            List<Move> randomMoves = await PostRandomMovesAsync();
            List<Move> expectedMoves = randomMoves;

            // when
            List<Move> actualMoves = await this.apiBroker.GetAllMovesAsync();

            // then
            foreach (Move expectedMove in expectedMoves)
            {
                Move actualMove = actualMoves.Single(approval => approval.Id == expectedMove.Id);
                actualMove.Should().BeEquivalentTo(expectedMove);
                await this.apiBroker.DeleteMoveByIdAsync(actualMove.Id);
            }
        }

        [Fact]
        public async Task ShouldGetMoveAsync()
        {
            // given
            Move randomMove = await PostRandomMoveAsync();
            Move expectedMove = randomMove;

            // when
            Move actualMove = await this.apiBroker.GetMoveByIdAsync(randomMove.Id);

            // then
            actualMove.Should().BeEquivalentTo(expectedMove);
            await this.apiBroker.DeleteMoveByIdAsync(actualMove.Id);
        }

        [Fact]
        public async Task ShouldPutMoveAsync()
        {
            // given
            Move randomMove = await PostRandomMoveAsync();
            Move modifiedMove = UpdateMoveWithRandomValues(randomMove);

            // when
            await this.apiBroker.PutMoveAsync(modifiedMove);
            Move actualMove = await this.apiBroker.GetMoveByIdAsync(randomMove.Id);

            // then
            actualMove.Should().BeEquivalentTo(modifiedMove);
            await this.apiBroker.DeleteMoveByIdAsync(actualMove.Id);
        }

        [Fact]
        public async Task ShouldDeleteMoveAsync()
        {
            // given
            Move randomMove = await PostRandomMoveAsync();
            Move inputMove = randomMove;
            Move expectedMove = inputMove;

            // when
            Move deletedMove =
                await this.apiBroker.DeleteMoveByIdAsync(inputMove.Id);

            ValueTask<Move> getMovebyIdTask =
                this.apiBroker.GetMoveByIdAsync(inputMove.Id);

            // then
            deletedMove.Should().BeEquivalentTo(expectedMove);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
                getMovebyIdTask.AsTask());
        }
    }
}
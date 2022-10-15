// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
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

        [Fact]
        public async Task ShouldGetAllMovesAsync()
        {
            // given
            Match randomMatch = await PostRandomMatchAsync();
            Player randomPlayer = await PostRandomPlayerAsync();
            List<Move> randomMoves = await PostRandomMovesAsync(randomMatch.Id, randomPlayer.Id);
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

            await this.apiBroker.DeletePlayerByIdAsync(randomPlayer.Id);
            await this.apiBroker.DeleteMatchByIdAsync(randomMatch.Id);
        }

        [Fact]
        public async Task ShouldGetMoveAsync()
        {
            // given
            Match randomMatch = await PostRandomMatchAsync();
            Player randomPlayer = await PostRandomPlayerAsync();
            Move randomMove = await PostRandomMoveAsync(randomMatch.Id, randomPlayer.Id);
            Move expectedMove = randomMove;

            // when
            Move actualMove = await this.apiBroker.GetMoveByIdAsync(randomMove.Id);

            // then
            actualMove.Should().BeEquivalentTo(expectedMove);
            await this.apiBroker.DeleteMoveByIdAsync(actualMove.Id);
            await this.apiBroker.DeletePlayerByIdAsync(randomPlayer.Id);
            await this.apiBroker.DeleteMatchByIdAsync(randomMatch.Id);
        }
    }
}
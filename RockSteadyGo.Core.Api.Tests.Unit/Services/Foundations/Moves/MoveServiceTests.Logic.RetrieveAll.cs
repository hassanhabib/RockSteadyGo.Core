// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using RockSteadyGo.Core.Api.Models.Moves;
using Xunit;

namespace RockSteadyGo.Core.Api.Tests.Unit.Services.Foundations.Moves
{
    public partial class MoveServiceTests
    {
        [Fact]
        public void ShouldReturnMoves()
        {
            // given
            IQueryable<Move> randomMoves = CreateRandomMoves();
            IQueryable<Move> storageMoves = randomMoves;
            IQueryable<Move> expectedMoves = storageMoves;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllMoves())
                    .Returns(storageMoves);

            // when
            IQueryable<Move> actualMoves =
                this.moveService.RetrieveAllMoves();

            // then
            actualMoves.Should().BeEquivalentTo(expectedMoves);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllMoves(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
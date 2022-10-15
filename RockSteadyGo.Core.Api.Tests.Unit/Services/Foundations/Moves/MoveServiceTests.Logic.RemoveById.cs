// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using RockSteadyGo.Core.Api.Models.Moves;
using Xunit;

namespace RockSteadyGo.Core.Api.Tests.Unit.Services.Foundations.Moves
{
    public partial class MoveServiceTests
    {
        [Fact]
        public async Task ShouldRemoveMoveByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputMoveId = randomId;
            Move randomMove = CreateRandomMove();
            Move storageMove = randomMove;
            Move expectedInputMove = storageMove;
            Move deletedMove = expectedInputMove;
            Move expectedMove = deletedMove.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMoveByIdAsync(inputMoveId))
                    .ReturnsAsync(storageMove);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteMoveAsync(expectedInputMove))
                    .ReturnsAsync(deletedMove);

            // when
            Move actualMove = await this.moveService
                .RemoveMoveByIdAsync(inputMoveId);

            // then
            actualMove.Should().BeEquivalentTo(expectedMove);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMoveByIdAsync(inputMoveId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteMoveAsync(expectedInputMove),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
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
        public async Task ShouldAddMoveAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            Move randomMove = CreateRandomMove(randomDateTimeOffset);
            Move inputMove = randomMove;
            Move storageMove = inputMove;
            Move expectedMove = storageMove.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertMoveAsync(inputMove))
                    .ReturnsAsync(storageMove);

            // when
            Move actualMove = await this.moveService
                .AddMoveAsync(inputMove);

            // then
            actualMove.Should().BeEquivalentTo(expectedMove);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertMoveAsync(inputMove),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
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
        public async Task ShouldModifyMoveAsync()
        {
            // given
            int randomDaysInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Move randomMove = CreateRandomMove(randomDateTimeOffset.AddDays(randomDaysInPast));
            Move storageMove = randomMove;

            Move randomModifiedMove = CreateRandomModifyMove(
                id: storageMove.Id,
                createdDate: storageMove.CreatedDate,
                updatedDate: randomDateTimeOffset);

            Move inputMove = randomModifiedMove;
            Move modifiedMove = inputMove;
            Move expectedMove = modifiedMove.DeepClone();
            Guid moveId = randomMove.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMoveByIdAsync(moveId))
                    .ReturnsAsync(storageMove);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateMoveAsync(inputMove))
                    .ReturnsAsync(modifiedMove);

            // when
            Move actualMove =
                await this.moveService.ModifyMoveAsync(inputMove);

            // then
            actualMove.Should().BeEquivalentTo(expectedMove);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMoveByIdAsync(inputMove.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateMoveAsync(inputMove),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
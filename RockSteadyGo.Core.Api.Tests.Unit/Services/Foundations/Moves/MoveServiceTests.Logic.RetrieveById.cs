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
        public async Task ShouldRetrieveMoveByIdAsync()
        {
            // given
            Move randomMove = CreateRandomMove();
            Move inputMove = randomMove;
            Move storageMove = randomMove;
            Move expectedMove = storageMove.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMoveByIdAsync(inputMove.Id))
                    .ReturnsAsync(storageMove);

            // when
            Move actualMove =
                await this.moveService.RetrieveMoveByIdAsync(inputMove.Id);

            // then
            actualMove.Should().BeEquivalentTo(expectedMove);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMoveByIdAsync(inputMove.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
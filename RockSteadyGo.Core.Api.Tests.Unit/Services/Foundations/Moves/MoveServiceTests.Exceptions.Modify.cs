// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using RockSteadyGo.Core.Api.Models.Moves;
using RockSteadyGo.Core.Api.Models.Moves.Exceptions;
using Xunit;

namespace RockSteadyGo.Core.Api.Tests.Unit.Services.Foundations.Moves
{
    public partial class MoveServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Move randomMove = CreateRandomMove();
            SqlException sqlException = GetSqlException();

            var failedMoveStorageException =
                new FailedMoveStorageException(sqlException);

            var expectedMoveDependencyException =
                new MoveDependencyException(failedMoveStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Move> modifyMoveTask =
                this.moveService.ModifyMoveAsync(randomMove);

            MoveDependencyException actualMoveDependencyException =
                await Assert.ThrowsAsync<MoveDependencyException>(
                    modifyMoveTask.AsTask);

            // then
            actualMoveDependencyException.Should()
                .BeEquivalentTo(expectedMoveDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMoveByIdAsync(randomMove.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedMoveDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateMoveAsync(randomMove),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
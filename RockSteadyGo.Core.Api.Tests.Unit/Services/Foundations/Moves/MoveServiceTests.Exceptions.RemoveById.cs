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
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Move randomMove = CreateRandomMove();
            SqlException sqlException = GetSqlException();

            var failedMoveStorageException =
                new FailedMoveStorageException(sqlException);

            var expectedMoveDependencyException =
                new MoveDependencyException(failedMoveStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMoveByIdAsync(randomMove.Id))
                    .Throws(sqlException);

            // when
            ValueTask<Move> addMoveTask =
                this.moveService.RemoveMoveByIdAsync(randomMove.Id);

            MoveDependencyException actualMoveDependencyException =
                await Assert.ThrowsAsync<MoveDependencyException>(
                    addMoveTask.AsTask);

            // then
            actualMoveDependencyException.Should()
                .BeEquivalentTo(expectedMoveDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMoveByIdAsync(randomMove.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedMoveDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteMoveAsync(It.IsAny<Move>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
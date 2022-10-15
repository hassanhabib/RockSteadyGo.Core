// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someMoveId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedMoveException =
                new LockedMoveException(databaseUpdateConcurrencyException);

            var expectedMoveDependencyValidationException =
                new MoveDependencyValidationException(lockedMoveException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMoveByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Move> removeMoveByIdTask =
                this.moveService.RemoveMoveByIdAsync(someMoveId);

            MoveDependencyValidationException actualMoveDependencyValidationException =
                await Assert.ThrowsAsync<MoveDependencyValidationException>(
                    removeMoveByIdTask.AsTask);

            // then
            actualMoveDependencyValidationException.Should()
                .BeEquivalentTo(expectedMoveDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMoveByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMoveDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteMoveAsync(It.IsAny<Move>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someMoveId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedMoveStorageException =
                new FailedMoveStorageException(sqlException);

            var expectedMoveDependencyException =
                new MoveDependencyException(failedMoveStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMoveByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Move> deleteMoveTask =
                this.moveService.RemoveMoveByIdAsync(someMoveId);

            MoveDependencyException actualMoveDependencyException =
                await Assert.ThrowsAsync<MoveDependencyException>(
                    deleteMoveTask.AsTask);

            // then
            actualMoveDependencyException.Should()
                .BeEquivalentTo(expectedMoveDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMoveByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedMoveDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
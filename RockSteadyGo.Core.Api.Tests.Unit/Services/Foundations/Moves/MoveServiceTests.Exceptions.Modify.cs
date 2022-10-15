// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMoveByIdAsync(randomMove.Id))
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

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMoveByIdAsync(randomMove.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedMoveDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateMoveAsync(randomMove),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            Move someMove = CreateRandomMove();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidMoveReferenceException =
                new InvalidMoveReferenceException(foreignKeyConstraintConflictException);

            MoveDependencyValidationException expectedMoveDependencyValidationException =
                new MoveDependencyValidationException(invalidMoveReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMoveByIdAsync(someMove.Id))
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<Move> modifyMoveTask =
                this.moveService.ModifyMoveAsync(someMove);

            MoveDependencyValidationException actualMoveDependencyValidationException =
                await Assert.ThrowsAsync<MoveDependencyValidationException>(
                    modifyMoveTask.AsTask);

            // then
            actualMoveDependencyValidationException.Should()
                .BeEquivalentTo(expectedMoveDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMoveByIdAsync(someMove.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMoveByIdAsync(someMove.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedMoveDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateMoveAsync(someMove),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
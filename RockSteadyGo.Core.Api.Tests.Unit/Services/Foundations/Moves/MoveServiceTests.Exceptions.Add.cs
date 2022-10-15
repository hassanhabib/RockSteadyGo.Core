// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Move someMove = CreateRandomMove();
            SqlException sqlException = GetSqlException();

            var failedMoveStorageException =
                new FailedMoveStorageException(sqlException);

            var expectedMoveDependencyException =
                new MoveDependencyException(failedMoveStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Move> addMoveTask =
                this.moveService.AddMoveAsync(someMove);

            MoveDependencyException actualMoveDependencyException =
                await Assert.ThrowsAsync<MoveDependencyException>(
                    addMoveTask.AsTask);

            // then
            actualMoveDependencyException.Should()
                .BeEquivalentTo(expectedMoveDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertMoveAsync(It.IsAny<Move>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedMoveDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfMoveAlreadyExsitsAndLogItAsync()
        {
            // given
            Move randomMove = CreateRandomMove();
            Move alreadyExistsMove = randomMove;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsMoveException =
                new AlreadyExistsMoveException(duplicateKeyException);

            var expectedMoveDependencyValidationException =
                new MoveDependencyValidationException(alreadyExistsMoveException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<Move> addMoveTask =
                this.moveService.AddMoveAsync(alreadyExistsMove);

            // then
            MoveDependencyValidationException actualMoveDependencyValidationException =
                await Assert.ThrowsAsync<MoveDependencyValidationException>(
                    addMoveTask.AsTask);

            actualMoveDependencyValidationException.Should()
                .BeEquivalentTo(expectedMoveDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertMoveAsync(It.IsAny<Move>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMoveDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            Move someMove = CreateRandomMove();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidMoveReferenceException =
                new InvalidMoveReferenceException(foreignKeyConstraintConflictException);

            var expectedMoveValidationException =
                new MoveDependencyValidationException(invalidMoveReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<Move> addMoveTask =
                this.moveService.AddMoveAsync(someMove);

            // then
            MoveDependencyValidationException actualMoveDependencyValidationException =
                await Assert.ThrowsAsync<MoveDependencyValidationException>(
                    addMoveTask.AsTask);

            actualMoveDependencyValidationException.Should()
                .BeEquivalentTo(expectedMoveValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMoveValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertMoveAsync(someMove),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Move someMove = CreateRandomMove();

            var databaseUpdateException =
                new DbUpdateException();

            var failedMoveStorageException =
                new FailedMoveStorageException(databaseUpdateException);

            var expectedMoveDependencyException =
                new MoveDependencyException(failedMoveStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Move> addMoveTask =
                this.moveService.AddMoveAsync(someMove);

            MoveDependencyException actualMoveDependencyException =
                await Assert.ThrowsAsync<MoveDependencyException>(
                    addMoveTask.AsTask);

            // then
            actualMoveDependencyException.Should()
                .BeEquivalentTo(expectedMoveDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertMoveAsync(It.IsAny<Move>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMoveDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Move someMove = CreateRandomMove();
            var serviceException = new Exception();

            var failedMoveServiceException =
                new FailedMoveServiceException(serviceException);

            var expectedMoveServiceException =
                new MoveServiceException(failedMoveServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<Move> addMoveTask =
                this.moveService.AddMoveAsync(someMove);

            MoveServiceException actualMoveServiceException =
                await Assert.ThrowsAsync<MoveServiceException>(
                    addMoveTask.AsTask);

            // then
            actualMoveServiceException.Should()
                .BeEquivalentTo(expectedMoveServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertMoveAsync(It.IsAny<Move>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMoveServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
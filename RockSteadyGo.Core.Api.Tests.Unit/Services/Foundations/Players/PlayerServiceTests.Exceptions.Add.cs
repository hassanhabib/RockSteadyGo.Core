using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using RockSteadyGo.Core.Api.Models.Players;
using RockSteadyGo.Core.Api.Models.Players.Exceptions;
using Xunit;

namespace RockSteadyGo.Core.Api.Tests.Unit.Services.Foundations.Players
{
    public partial class PlayerServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Player somePlayer = CreateRandomPlayer();
            SqlException sqlException = GetSqlException();

            var failedPlayerStorageException =
                new FailedPlayerStorageException(sqlException);

            var expectedPlayerDependencyException =
                new PlayerDependencyException(failedPlayerStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Player> addPlayerTask =
                this.playerService.AddPlayerAsync(somePlayer);

            PlayerDependencyException actualPlayerDependencyException =
                await Assert.ThrowsAsync<PlayerDependencyException>(
                    addPlayerTask.AsTask);

            // then
            actualPlayerDependencyException.Should()
                .BeEquivalentTo(expectedPlayerDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPlayerAsync(It.IsAny<Player>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPlayerDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfPlayerAlreadyExsitsAndLogItAsync()
        {
            // given
            Player randomPlayer = CreateRandomPlayer();
            Player alreadyExistsPlayer = randomPlayer;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsPlayerException =
                new AlreadyExistsPlayerException(duplicateKeyException);

            var expectedPlayerDependencyValidationException =
                new PlayerDependencyValidationException(alreadyExistsPlayerException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<Player> addPlayerTask =
                this.playerService.AddPlayerAsync(alreadyExistsPlayer);

            // then
            PlayerDependencyValidationException actualPlayerDependencyValidationException =
                await Assert.ThrowsAsync<PlayerDependencyValidationException>(
                    addPlayerTask.AsTask);

            actualPlayerDependencyValidationException.Should()
                .BeEquivalentTo(expectedPlayerDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPlayerAsync(It.IsAny<Player>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            Player somePlayer = CreateRandomPlayer();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidPlayerReferenceException =
                new InvalidPlayerReferenceException(foreignKeyConstraintConflictException);

            var expectedPlayerValidationException =
                new PlayerDependencyValidationException(invalidPlayerReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<Player> addPlayerTask =
                this.playerService.AddPlayerAsync(somePlayer);

            // then
            PlayerDependencyValidationException actualPlayerDependencyValidationException =
                await Assert.ThrowsAsync<PlayerDependencyValidationException>(
                    addPlayerTask.AsTask);

            actualPlayerDependencyValidationException.Should()
                .BeEquivalentTo(expectedPlayerValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPlayerAsync(somePlayer),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Player somePlayer = CreateRandomPlayer();

            var databaseUpdateException =
                new DbUpdateException();

            var failedPlayerStorageException =
                new FailedPlayerStorageException(databaseUpdateException);

            var expectedPlayerDependencyException =
                new PlayerDependencyException(failedPlayerStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Player> addPlayerTask =
                this.playerService.AddPlayerAsync(somePlayer);

            PlayerDependencyException actualPlayerDependencyException =
                await Assert.ThrowsAsync<PlayerDependencyException>(
                    addPlayerTask.AsTask);

            // then
            actualPlayerDependencyException.Should()
                .BeEquivalentTo(expectedPlayerDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPlayerAsync(It.IsAny<Player>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Player somePlayer = CreateRandomPlayer();
            var serviceException = new Exception();

            var failedPlayerServiceException =
                new FailedPlayerServiceException(serviceException);

            var expectedPlayerServiceException =
                new PlayerServiceException(failedPlayerServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<Player> addPlayerTask =
                this.playerService.AddPlayerAsync(somePlayer);

            PlayerServiceException actualPlayerServiceException =
                await Assert.ThrowsAsync<PlayerServiceException>(
                    addPlayerTask.AsTask);

            // then
            actualPlayerServiceException.Should()
                .BeEquivalentTo(expectedPlayerServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPlayerAsync(It.IsAny<Player>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
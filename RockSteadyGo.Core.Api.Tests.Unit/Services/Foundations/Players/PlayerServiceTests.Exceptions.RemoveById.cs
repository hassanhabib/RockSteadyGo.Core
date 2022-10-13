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
using RockSteadyGo.Core.Api.Models.Players;
using RockSteadyGo.Core.Api.Models.Players.Exceptions;
using Xunit;

namespace RockSteadyGo.Core.Api.Tests.Unit.Services.Foundations.Players
{
    public partial class PlayerServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Player randomPlayer = CreateRandomPlayer();
            SqlException sqlException = GetSqlException();

            var failedPlayerStorageException =
                new FailedPlayerStorageException(sqlException);

            var expectedPlayerDependencyException =
                new PlayerDependencyException(failedPlayerStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPlayerByIdAsync(randomPlayer.Id))
                    .Throws(sqlException);

            // when
            ValueTask<Player> addPlayerTask =
                this.playerService.RemovePlayerByIdAsync(randomPlayer.Id);

            PlayerDependencyException actualPlayerDependencyException =
                await Assert.ThrowsAsync<PlayerDependencyException>(
                    addPlayerTask.AsTask);

            // then
            actualPlayerDependencyException.Should()
                .BeEquivalentTo(expectedPlayerDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPlayerByIdAsync(randomPlayer.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPlayerDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePlayerAsync(It.IsAny<Player>()),
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
            Guid somePlayerId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedPlayerException =
                new LockedPlayerException(databaseUpdateConcurrencyException);

            var expectedPlayerDependencyValidationException =
                new PlayerDependencyValidationException(lockedPlayerException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPlayerByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Player> removePlayerByIdTask =
                this.playerService.RemovePlayerByIdAsync(somePlayerId);

            PlayerDependencyValidationException actualPlayerDependencyValidationException =
                await Assert.ThrowsAsync<PlayerDependencyValidationException>(
                    removePlayerByIdTask.AsTask);

            // then
            actualPlayerDependencyValidationException.Should()
                .BeEquivalentTo(expectedPlayerDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPlayerByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePlayerAsync(It.IsAny<Player>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid somePlayerId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedPlayerStorageException =
                new FailedPlayerStorageException(sqlException);

            var expectedPlayerDependencyException =
                new PlayerDependencyException(failedPlayerStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPlayerByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Player> deletePlayerTask =
                this.playerService.RemovePlayerByIdAsync(somePlayerId);

            PlayerDependencyException actualPlayerDependencyException =
                await Assert.ThrowsAsync<PlayerDependencyException>(
                    deletePlayerTask.AsTask);

            // then
            actualPlayerDependencyException.Should()
                .BeEquivalentTo(expectedPlayerDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPlayerByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPlayerDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid somePlayerId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedPlayerServiceException =
                new FailedPlayerServiceException(serviceException);

            var expectedPlayerServiceException =
                new PlayerServiceException(failedPlayerServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPlayerByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Player> removePlayerByIdTask =
                this.playerService.RemovePlayerByIdAsync(somePlayerId);

            PlayerServiceException actualPlayerServiceException =
                await Assert.ThrowsAsync<PlayerServiceException>(
                    removePlayerByIdTask.AsTask);

            // then
            actualPlayerServiceException.Should()
                .BeEquivalentTo(expectedPlayerServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPlayerByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
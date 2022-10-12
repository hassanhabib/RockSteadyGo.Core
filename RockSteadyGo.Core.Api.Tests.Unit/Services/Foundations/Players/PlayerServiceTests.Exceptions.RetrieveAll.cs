// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using RockSteadyGo.Core.Api.Models.Players.Exceptions;
using Xunit;

namespace RockSteadyGo.Core.Api.Tests.Unit.Services.Foundations.Players
{
    public partial class PlayerServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedPlayerStorageException(sqlException);

            var expectedPlayerDependencyException =
                new PlayerDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPlayers())
                    .Throws(sqlException);

            // when
            Action retrieveAllPlayersAction = () =>
                this.playerService.RetrieveAllPlayers();

            PlayerDependencyException actualPlayerDependencyException =
                Assert.Throws<PlayerDependencyException>(retrieveAllPlayersAction);

            // then
            actualPlayerDependencyException.Should()
                .BeEquivalentTo(expectedPlayerDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPlayers(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPlayerDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
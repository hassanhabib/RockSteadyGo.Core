// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using RockSteadyGo.Core.Api.Models.Moves.Exceptions;
using Xunit;

namespace RockSteadyGo.Core.Api.Tests.Unit.Services.Foundations.Moves
{
    public partial class MoveServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedMoveStorageException(sqlException);

            var expectedMoveDependencyException =
                new MoveDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllMoves())
                    .Throws(sqlException);

            // when
            Action retrieveAllMovesAction = () =>
                this.moveService.RetrieveAllMoves();

            MoveDependencyException actualMoveDependencyException =
                Assert.Throws<MoveDependencyException>(retrieveAllMovesAction);

            // then
            actualMoveDependencyException.Should()
                .BeEquivalentTo(expectedMoveDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllMoves(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedMoveDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);

            var failedMoveServiceException =
                new FailedMoveServiceException(serviceException);

            var expectedMoveServiceException =
                new MoveServiceException(failedMoveServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllMoves())
                    .Throws(serviceException);

            // when
            Action retrieveAllMovesAction = () =>
                this.moveService.RetrieveAllMoves();

            MoveServiceException actualMoveServiceException =
                Assert.Throws<MoveServiceException>(retrieveAllMovesAction);

            // then
            actualMoveServiceException.Should()
                .BeEquivalentTo(expectedMoveServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllMoves(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMoveServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
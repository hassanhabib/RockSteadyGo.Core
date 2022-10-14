using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using RockSteadyGo.Core.Api.Models.Matches.Exceptions;
using Xunit;

namespace RockSteadyGo.Core.Api.Tests.Unit.Services.Foundations.Matches
{
    public partial class MatchServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedMatchStorageException(sqlException);

            var expectedMatchDependencyException =
                new MatchDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllMatches())
                    .Throws(sqlException);

            // when
            Action retrieveAllMatchesAction = () =>
                this.matchService.RetrieveAllMatches();

            MatchDependencyException actualMatchDependencyException =
                Assert.Throws<MatchDependencyException>(retrieveAllMatchesAction);

            // then
            actualMatchDependencyException.Should()
                .BeEquivalentTo(expectedMatchDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllMatches(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedMatchDependencyException))),
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

            var failedMatchServiceException =
                new FailedMatchServiceException(serviceException);

            var expectedMatchServiceException =
                new MatchServiceException(failedMatchServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllMatches())
                    .Throws(serviceException);

            // when
            Action retrieveAllMatchesAction = () =>
                this.matchService.RetrieveAllMatches();

            MatchServiceException actualMatchServiceException =
                Assert.Throws<MatchServiceException>(retrieveAllMatchesAction);

            // then
            actualMatchServiceException.Should()
                .BeEquivalentTo(expectedMatchServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllMatches(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMatchServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
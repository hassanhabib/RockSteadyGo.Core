// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using Xunit;
using Match = RockSteadyGo.Core.Api.Models.Matches.Match;

namespace RockSteadyGo.Core.Api.Tests.Unit.Services.Foundations.Matches
{
    public partial class MatchServiceTests
    {
        [Fact]
        public void ShouldReturnMatches()
        {
            // given
            IQueryable<Match> randomMatches = CreateRandomMatches();
            IQueryable<Match> storageMatches = randomMatches;
            IQueryable<Match> expectedMatches = storageMatches;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllMatches())
                    .Returns(storageMatches);

            // when
            IQueryable<Match> actualMatches =
                this.matchService.RetrieveAllMatches();

            // then
            actualMatches.Should().BeEquivalentTo(expectedMatches);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllMatches(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
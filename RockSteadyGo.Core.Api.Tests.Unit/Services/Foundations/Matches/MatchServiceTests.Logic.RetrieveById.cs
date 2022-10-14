// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;
using Match = RockSteadyGo.Core.Api.Models.Matches.Match;

namespace RockSteadyGo.Core.Api.Tests.Unit.Services.Foundations.Matches
{
    public partial class MatchServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveMatchByIdAsync()
        {
            // given
            Match randomMatch = CreateRandomMatch();
            Match inputMatch = randomMatch;
            Match storageMatch = randomMatch;
            Match expectedMatch = storageMatch.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMatchByIdAsync(inputMatch.Id))
                    .ReturnsAsync(storageMatch);

            // when
            Match actualMatch =
                await this.matchService.RetrieveMatchByIdAsync(inputMatch.Id);

            // then
            actualMatch.Should().BeEquivalentTo(expectedMatch);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMatchByIdAsync(inputMatch.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
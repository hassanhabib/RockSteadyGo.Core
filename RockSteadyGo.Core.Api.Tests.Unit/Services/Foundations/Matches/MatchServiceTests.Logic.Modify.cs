// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
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
        public async Task ShouldModifyMatchAsync()
        {
            // given
            int randomDaysInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Match randomMatch = CreateRandomMatch(randomDateTimeOffset.AddDays(randomDaysInPast));
            Match storageMatch = randomMatch;

            Match randomModifiedMatch = CreateRandomModifyMatch(
                id: storageMatch.Id,
                createdDate: storageMatch.CreatedDate,
                updatedDate: randomDateTimeOffset);


            Match inputMatch = randomModifiedMatch;
            Match modifiedMacth = inputMatch;
            Match expectedMatch = modifiedMacth.DeepClone();
            Guid matchId = inputMatch.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMatchByIdAsync(matchId))
                    .ReturnsAsync(storageMatch);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateMatchAsync(inputMatch))
                    .ReturnsAsync(modifiedMacth);

            // when
            Match actualMatch =
                await this.matchService.ModifyMatchAsync(inputMatch);

            // then
            actualMatch.Should().BeEquivalentTo(expectedMatch);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMatchByIdAsync(inputMatch.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateMatchAsync(inputMatch),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
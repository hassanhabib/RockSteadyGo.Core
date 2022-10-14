// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
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
        public async Task ShouldRemoveMatchByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputMatchId = randomId;
            Match randomMatch = CreateRandomMatch();
            Match storageMatch = randomMatch;
            Match expectedInputMatch = storageMatch;
            Match deletedMatch = expectedInputMatch;
            Match expectedMatch = deletedMatch.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectMatchByIdAsync(inputMatchId))
                    .ReturnsAsync(storageMatch);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteMatchAsync(expectedInputMatch))
                    .ReturnsAsync(deletedMatch);

            // when
            Match actualMatch = await this.matchService
                .RemoveMatchByIdAsync(inputMatchId);

            // then
            actualMatch.Should().BeEquivalentTo(expectedMatch);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectMatchByIdAsync(inputMatchId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteMatchAsync(expectedInputMatch),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
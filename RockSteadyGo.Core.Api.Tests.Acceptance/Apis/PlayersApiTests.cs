// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RockSteadyGo.Core.Api.Tests.Acceptance.Brokers;
using RockSteadyGo.Core.Api.Tests.Acceptance.Models.Players;
using Tynamix.ObjectFiller;
using Xunit;

namespace RockSteadyGo.Core.Api.Tests.Acceptance.Apis.Players
{
    [Collection(nameof(ApiTestCollection))]
    public partial class PlayersApiTests
    {
        private readonly ApiBroker apiBroker;

        public PlayersApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Player UpdatePlayerWithRandomValues(Player inputPlayer)
        {
            Player randomUpdatedPlayer = CreateRandomPlayer();
            randomUpdatedPlayer.Id = inputPlayer.Id;
            randomUpdatedPlayer.CreatedDate = inputPlayer.CreatedDate;

            return randomUpdatedPlayer;
        }

        private async ValueTask<Player> PostRandomPlayerAsync()
        {
            Player randomPlayer = CreateRandomPlayer();
            await this.apiBroker.PostPlayerAsync(randomPlayer);

            return randomPlayer;
        }

        private async ValueTask<List<Player>> PostRandomPlayersAsync()
        {
            int randomNumber = GetRandomNumber();
            var randomPlayers = new List<Player>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomPlayers.Add(await PostRandomPlayerAsync());
            }

            return randomPlayers;
        }

        private static Player CreateRandomPlayer() =>
            CreateRandomPlayerFiller().Create();

        private static Filler<Player> CreateRandomPlayerFiller()
        {
            Guid userId = Guid.NewGuid();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<Player>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(player => player.CreatedDate).Use(now);

            return filler;
        }
    }
}
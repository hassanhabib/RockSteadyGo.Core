// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using RockSteadyGo.Core.Api.Brokers.DateTimes;
using RockSteadyGo.Core.Api.Brokers.Loggings;
using RockSteadyGo.Core.Api.Brokers.Storages;
using RockSteadyGo.Core.Api.Models.Players;

namespace RockSteadyGo.Core.Api.Services.Foundations.Players
{
    public partial class PlayerService : IPlayerService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public PlayerService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Player> AddPlayerAsync(Player player) =>
            TryCatch(async () =>
            {
                ValidatePlayerOnAdd(player);

                return await this.storageBroker.InsertPlayerAsync(player);
            });

        public IQueryable<Player> RetrieveAllPlayers() =>
            TryCatch(() => this.storageBroker.SelectAllPlayers());

        public ValueTask<Player> RetrievePlayerByIdAsync(Guid playerId) =>
            TryCatch(async () =>
            {
                ValidatePlayerId(playerId);

                Player maybePlayer = await this.storageBroker
                    .SelectPlayerByIdAsync(playerId);

                ValidateStoragePlayer(maybePlayer, playerId);

                return maybePlayer;
            });

        public ValueTask<Player> ModifyPlayerAsync(Player player) =>
            TryCatch(async () =>
            {
                ValidatePlayerOnModify(player);

                Player maybePlayer =
                    await this.storageBroker.SelectPlayerByIdAsync(player.Id);

                ValidateStoragePlayer(maybePlayer, player.Id);
                ValidateAgainstStoragePlayerOnModify(inputPlayer: player, storagePlayer: maybePlayer);

                return await this.storageBroker.UpdatePlayerAsync(player);
            });

        public ValueTask<Player> RemovePlayerByIdAsync(Guid playerId) =>
            throw new NotImplementedException();
    }
}

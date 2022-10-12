// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

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
            this.storageBroker.SelectAllPlayers();
    }
}

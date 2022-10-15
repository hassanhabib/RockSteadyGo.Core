// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using RockSteadyGo.Core.Api.Brokers.DateTimes;
using RockSteadyGo.Core.Api.Brokers.Loggings;
using RockSteadyGo.Core.Api.Brokers.Storages;
using RockSteadyGo.Core.Api.Models.Moves;

namespace RockSteadyGo.Core.Api.Services.Foundations.Moves
{
    public partial class MoveService : IMoveService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public MoveService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Move> AddMoveAsync(Move move) =>
            TryCatch(async () =>
            {
                ValidateMoveOnAdd(move);

                return await this.storageBroker.InsertMoveAsync(move);
            });

        public IQueryable<Move> RetrieveAllMoves() =>
            TryCatch(() => this.storageBroker.SelectAllMoves());
    }
}
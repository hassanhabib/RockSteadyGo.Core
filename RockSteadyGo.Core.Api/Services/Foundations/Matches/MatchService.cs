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
using RockSteadyGo.Core.Api.Models.Matches;

namespace RockSteadyGo.Core.Api.Services.Foundations.Matches
{
    public partial class MatchService : IMatchService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public MatchService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Match> AddMatchAsync(Match match) =>
            TryCatch(async () =>
            {
                ValidateMatchOnAdd(match);

                return await this.storageBroker.InsertMatchAsync(match);
            });

        public IQueryable<Match> RetrieveAllMatches() =>
            TryCatch(() => this.storageBroker.SelectAllMatches());

        public ValueTask<Match> RetrieveMatchByIdAsync(Guid matchId) =>
            TryCatch(async () =>
            {
                ValidateMatchId(matchId);

                Match maybeMatch = await this.storageBroker
                    .SelectMatchByIdAsync(matchId);

                ValidateStorageMatch(maybeMatch, matchId);

                return maybeMatch;
            });

        public ValueTask<Match> ModifyMatchAsync(Match match) =>
            TryCatch(async () =>
            {
                ValidateMatchOnModify(match);

                Match maybeMatch =
                    await this.storageBroker.SelectMatchByIdAsync(match.Id);

                ValidateStorageMatch(maybeMatch, match.Id);
                ValidateAgainstStorageMatchOnModify(inputMatch: match, storageMatch: maybeMatch);

                return await this.storageBroker.UpdateMatchAsync(match);
            });

        public ValueTask<Match> RemoveMatchByIdAsync(Guid matchId) =>
            TryCatch(async () =>
            {
                ValidateMatchId(matchId);

                Match maybeMatch = await this.storageBroker
                    .SelectMatchByIdAsync(matchId);

                return await this.storageBroker.DeleteMatchAsync(maybeMatch);
            });
    }
}
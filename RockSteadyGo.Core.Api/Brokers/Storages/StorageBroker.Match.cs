// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RockSteadyGo.Core.Api.Models.Matches;

namespace RockSteadyGo.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Match> Matches { get; set; }

        public async ValueTask<Match> InsertMatchAsync(Match match) =>
            await InsertAsync(match);

        public IQueryable<Match> SelectAllMatches() => SelectAll<Match>();

        public async ValueTask<Match> SelectMatchByIdAsync(Guid matchId) =>
            await SelectAsync<Match>(matchId);

        public async ValueTask<Match> UpdateMatchAsync(Match match) =>
            await UpdateAsync(match);

        public async ValueTask<Match> DeleteMatchAsync(Match match) =>
            await DeleteAsync(match);
    }
}

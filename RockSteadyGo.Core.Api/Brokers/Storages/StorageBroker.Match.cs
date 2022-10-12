// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RockSteadyGo.Core.Api.Models.Matches;

namespace RockSteadyGo.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Match> Matches { get; set; }

        public async ValueTask<Match> InsertMatchAsync(Match match)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Match> matchEntityEntry =
                await broker.Matches.AddAsync(match);

            await broker.SaveChangesAsync();

            return matchEntityEntry.Entity;
        }

        public IQueryable<Match> SelectAllMatches()
        {
            using var broker =
                new StorageBroker(this.configuration);

            return broker.Matches;
        }

        public async ValueTask<Match> SelectMatchByIdAsync(Guid matchId)
        {
            using var broker =
                new StorageBroker(this.configuration);

            return await broker.Matches.FindAsync(matchId);
        }

        public async ValueTask<Match> UpdateMatchAsync(Match match)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Match> matchEntityEntry =
                broker.Matches.Update(match);

            await broker.SaveChangesAsync();

            return matchEntityEntry.Entity;
        }

        public async ValueTask<Match> DeleteMatchAsync(Match match)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Match> matchEntityEntry =
                broker.Matches.Remove(match);

            await broker.SaveChangesAsync();

            return matchEntityEntry.Entity;
        }
    }
}

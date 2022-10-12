// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

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
    }
}

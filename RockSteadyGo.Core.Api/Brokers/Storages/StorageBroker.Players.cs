using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RockSteadyGo.Core.Api.Models.Players;

namespace RockSteadyGo.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Player> Players { get; set; }

        public async ValueTask<Player> InsertPlayerAsync(Player player)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Player> playerEntityEntry =
                await broker.Players.AddAsync(player);

            await broker.SaveChangesAsync();

            return playerEntityEntry.Entity;
        }

        public IQueryable<Player> SelectAllPlayers()
        {
            using var broker =
                new StorageBroker(this.configuration);

            return broker.Players;
        }

        public async ValueTask<Player> SelectPlayerByIdAsync(Guid playerId)
        {
            using var broker =
                new StorageBroker(this.configuration);

            return await broker.Players.FindAsync(playerId);
        }

        public async ValueTask<Player> UpdatePlayerAsync(Player player)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Player> playerEntityEntry =
                broker.Players.Update(player);

            await broker.SaveChangesAsync();

            return playerEntityEntry.Entity;
        }

        public async ValueTask<Player> DeletePlayerAsync(Player player)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Player> playerEntityEntry =
                broker.Players.Remove(player);

            await broker.SaveChangesAsync();

            return playerEntityEntry.Entity;
        }
    }
}
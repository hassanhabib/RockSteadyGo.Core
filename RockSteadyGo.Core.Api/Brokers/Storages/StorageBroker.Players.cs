// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RockSteadyGo.Core.Api.Models.Players;

namespace RockSteadyGo.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Player> Players { get; set; }

        public async ValueTask<Player> InsertPlayerAsync(Player player) =>
            await InsertAsync(player);

        public IQueryable<Player> SelectAllPlayers() => SelectAll<Player>();

        public async ValueTask<Player> SelectPlayerByIdAsync(Guid playerId) =>
            await SelectAsync<Player>(playerId);

        public async ValueTask<Player> UpdatePlayerAsync(Player player) =>
            await UpdateAsync(player);

        public async ValueTask<Player> DeletePlayerAsync(Player player) =>
            await DeleteAsync(player);
    }
}

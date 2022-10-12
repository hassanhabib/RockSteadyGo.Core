// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using RockSteadyGo.Core.Api.Models.Players;

namespace RockSteadyGo.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Player> InsertPlayerAsync(Player player);
        IQueryable<Player> SelectAllPlayers();
        ValueTask<Player> SelectPlayerByIdAsync(Guid playerId);
        ValueTask<Player> UpdatePlayerAsync(Player player);
    }
}

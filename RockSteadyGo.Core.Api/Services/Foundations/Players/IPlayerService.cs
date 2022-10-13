// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using RockSteadyGo.Core.Api.Models.Players;

namespace RockSteadyGo.Core.Api.Services.Foundations.Players
{
    public interface IPlayerService
    {
        ValueTask<Player> AddPlayerAsync(Player player);
        IQueryable<Player> RetrieveAllPlayers();
        ValueTask<Player> RetrievePlayerByIdAsync(Guid playerId);
        ValueTask<Player> ModifyPlayerAsync(Player player);
        ValueTask<Player> RemovePlayerByIdAsync(Guid playerId);
    }
}

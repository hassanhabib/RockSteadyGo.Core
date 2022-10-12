// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using RockSteadyGo.Core.Api.Models.Players;

namespace RockSteadyGo.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Player> Matches { get; set; }
    }
}

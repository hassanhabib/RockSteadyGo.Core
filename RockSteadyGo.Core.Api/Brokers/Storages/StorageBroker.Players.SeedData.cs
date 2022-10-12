// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RockSteadyGo.Core.Api.Models.Players;

namespace RockSteadyGo.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void AddPlayersSeedData(ModelBuilder modelBuilder)
        {
            var jsonData = File.ReadAllText(@"Resources\Players.json");
            List<Player> allPlayers = JsonConvert.DeserializeObject<List<Player>>(jsonData);
            modelBuilder.Entity<Player>().HasData(allPlayers);
        }
    }
}

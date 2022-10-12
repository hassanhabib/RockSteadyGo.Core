// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RockSteadyGo.Core.Api.Models.Matches;

namespace RockSteadyGo.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void AddMatchesSeedData(ModelBuilder modelBuilder)
        {
            string jsonData = File.ReadAllText(@"Resources\Matches.json");
            List<Match> allMatches = JsonConvert.DeserializeObject<List<Match>>(jsonData);
            modelBuilder.Entity<Match>().HasData(allMatches);
        }
    }
}

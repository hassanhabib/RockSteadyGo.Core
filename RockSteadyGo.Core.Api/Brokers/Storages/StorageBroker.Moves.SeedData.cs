// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RockSteadyGo.Core.Api.Models.Moves;

namespace RockSteadyGo.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void AddMovesSeedData(ModelBuilder modelBuilder)
        {
            string jsonData = File.ReadAllText(@"Resources\Moves.json");
            List<Move> allMoves = JsonConvert.DeserializeObject<List<Move>>(jsonData);
            modelBuilder.Entity<Move>().HasData(allMoves);
        }
    }
}

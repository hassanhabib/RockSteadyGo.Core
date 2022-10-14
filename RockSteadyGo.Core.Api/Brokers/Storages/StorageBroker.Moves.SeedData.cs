// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
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

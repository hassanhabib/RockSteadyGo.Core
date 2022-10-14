// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using RockSteadyGo.Core.Api.Models.Moves;

namespace RockSteadyGo.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void AddMoveConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Move>()
                .HasOne(move => move.Player)
                .WithMany(player => player.Moves)
                .HasForeignKey(move => move.PlayerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Move>()
                .HasOne(move => move.Match)
                .WithMany(match => match.Moves)
                .HasForeignKey(move => move.MatchId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

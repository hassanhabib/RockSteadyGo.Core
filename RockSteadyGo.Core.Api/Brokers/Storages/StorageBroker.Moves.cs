// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RockSteadyGo.Core.Api.Models.Moves;

namespace RockSteadyGo.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Move> Moves { get; set; }

        public async ValueTask<Move> InsertMoveAsync(Move move) =>
            await InsertAsync(move);

        public IQueryable<Move> SelectAllMoves() => SelectAll<Move>();

        public async ValueTask<Move> SelectMoveByIdAsync(Guid moveId) =>
            await SelectAsync<Move>(moveId);

        public async ValueTask<Move> UpdateMoveAsync(Move move) =>
            await UpdateAsync(move);

        public async ValueTask<Move> DeleteMoveAsync(Move move) =>
            await DeleteAsync(move);
    }
}

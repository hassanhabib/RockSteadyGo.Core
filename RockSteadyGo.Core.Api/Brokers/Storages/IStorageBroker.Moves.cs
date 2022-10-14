// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using RockSteadyGo.Core.Api.Models.Moves;

namespace RockSteadyGo.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Move> InsertMoveAsync(Move move);
        IQueryable<Move> SelectAllMoves();
        ValueTask<Move> SelectMoveByIdAsync(Guid moveId);
        ValueTask<Move> UpdateMoveAsync(Move move);
        ValueTask<Move> DeleteMoveAsync(Move move);
    }
}

// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using RockSteadyGo.Core.Api.Models.Moves;

namespace RockSteadyGo.Core.Api.Services.Foundations.Moves
{
    public interface IMoveService
    {
        ValueTask<Move> AddMoveAsync(Move move);
        IQueryable<Move> RetrieveAllMoves();
    }
}
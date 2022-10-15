// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using RockSteadyGo.Core.Api.Models.Moves;
using RockSteadyGo.Core.Api.Models.Moves.Exceptions;

namespace RockSteadyGo.Core.Api.Services.Foundations.Moves
{
    public partial class MoveService
    {
        private void ValidateMoveOnAdd(Move move)
        {
            ValidateMoveIsNotNull(move);
        }

        private static void ValidateMoveIsNotNull(Move move)
        {
            if (move is null)
            {
                throw new NullMoveException();
            }
        }
    }
}
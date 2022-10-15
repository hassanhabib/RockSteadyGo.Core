// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Moves.Exceptions
{
    public class InvalidMoveException : Xeption
    {
        public InvalidMoveException()
            : base(message: "Invalid move. Please correct the errors and try again.")
        { }
    }
}
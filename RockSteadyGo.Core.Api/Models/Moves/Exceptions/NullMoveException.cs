// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Moves.Exceptions
{
    public class NullMoveException : Xeption
    {
        public NullMoveException()
            : base(message: "Move is null.")
        { }
    }
}
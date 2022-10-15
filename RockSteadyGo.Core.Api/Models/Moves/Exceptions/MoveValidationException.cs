// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Moves.Exceptions
{
    public class MoveValidationException : Xeption
    {
        public MoveValidationException(Xeption innerException)
            : base(message: "Move validation errors occurred, please try again.",
                  innerException)
        { }
    }
}
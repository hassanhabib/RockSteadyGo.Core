// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Moves.Exceptions
{
    public class MoveDependencyValidationException : Xeption
    {
        public MoveDependencyValidationException(Xeption innerException)
            : base(message: "Move dependency validation occurred, please try again.", innerException)
        { }
    }
}
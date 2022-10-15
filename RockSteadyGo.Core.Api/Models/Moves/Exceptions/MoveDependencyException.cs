// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Moves.Exceptions
{
    public class MoveDependencyException : Xeption
    {
        public MoveDependencyException(Xeption innerException) :
            base(message: "Move dependency error occurred, contact support.", innerException)
        { }
    }
}
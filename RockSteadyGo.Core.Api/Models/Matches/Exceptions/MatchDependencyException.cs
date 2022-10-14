// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Matches.Exceptions
{
    public class MatchDependencyException : Xeption
    {
        public MatchDependencyException(Xeption innerException) :
            base(message: "Match dependency error occurred, contact support.", innerException)
        { }
    }
}
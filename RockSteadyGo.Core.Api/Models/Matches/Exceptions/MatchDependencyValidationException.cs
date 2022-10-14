// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Matches.Exceptions
{
    public class MatchDependencyValidationException : Xeption
    {
        public MatchDependencyValidationException(Xeption innerException)
            : base(message: "Match dependency validation occurred, please try again.", innerException)
        { }
    }
}
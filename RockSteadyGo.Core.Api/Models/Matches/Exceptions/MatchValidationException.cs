// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Matches.Exceptions
{
    public class MatchValidationException : Xeption
    {
        public MatchValidationException(Xeption innerException)
            : base(message: "Match validation errors occurred, please try again.",
                  innerException)
        { }
    }
}
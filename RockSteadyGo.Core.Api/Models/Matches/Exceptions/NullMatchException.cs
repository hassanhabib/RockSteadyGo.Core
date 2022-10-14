// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Matches.Exceptions
{
    public class NullMatchException : Xeption
    {
        public NullMatchException()
            : base(message: "Match is null.")
        { }
    }
}
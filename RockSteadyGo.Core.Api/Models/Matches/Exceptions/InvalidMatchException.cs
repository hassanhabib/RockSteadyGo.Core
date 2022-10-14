// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Matches.Exceptions
{
    public class InvalidMatchException : Xeption
    {
        public InvalidMatchException()
            : base(message: "Invalid match. Please correct the errors and try again.")
        { }
    }
}
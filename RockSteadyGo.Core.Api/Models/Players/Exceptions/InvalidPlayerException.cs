// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Players.Exceptions
{
    public class InvalidPlayerException : Xeption
    {
        public InvalidPlayerException()
            : base(message: "Invalid player. Please correct the errors and try again.")
        { }
    }
}
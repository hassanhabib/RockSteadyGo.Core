// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Players.Exceptions
{
    public class PlayerValidationException : Xeption
    {
        public PlayerValidationException(Xeption innerException)
            : base(message: "Player validation errors occurred, please try again.",
                  innerException)
        { }
    }
}
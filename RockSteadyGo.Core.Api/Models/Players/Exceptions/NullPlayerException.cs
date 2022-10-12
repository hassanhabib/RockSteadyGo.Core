// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Players.Exceptions
{
    public class NullPlayerException : Xeption
    {
        public NullPlayerException()
            : base(message: "Player is null.")
        { }
    }
}

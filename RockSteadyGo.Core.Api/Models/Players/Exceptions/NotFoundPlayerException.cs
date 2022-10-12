// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Players.Exceptions
{
    public class NotFoundPlayerException : Xeption
    {
        public NotFoundPlayerException(Guid playerId)
            : base(message: $"Couldn't find player with playerId: {playerId}.")
        { }
    }
}
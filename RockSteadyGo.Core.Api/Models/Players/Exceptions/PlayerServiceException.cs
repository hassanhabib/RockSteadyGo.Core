// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Players.Exceptions
{
    public class PlayerServiceException : Xeption
    {
        public PlayerServiceException(Exception innerException)
            : base(message: "Player service error occurred, contact support.", innerException)
        { }
    }
}
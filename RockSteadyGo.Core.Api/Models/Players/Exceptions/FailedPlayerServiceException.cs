// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Players.Exceptions
{
    public class FailedPlayerServiceException : Xeption
    {
        public FailedPlayerServiceException(Exception innerException)
            : base(message: "Failed player service occurred, please contact support", innerException)
        { }
    }
}
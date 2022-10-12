// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Players.Exceptions
{
    public class FailedPlayerStorageException : Xeption
    {
        public FailedPlayerStorageException(Exception innerException)
            : base(message: "Failed player storage error occurred, contact support.", innerException)
        { }
    }
}
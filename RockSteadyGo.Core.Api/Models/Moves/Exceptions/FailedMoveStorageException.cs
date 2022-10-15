// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Moves.Exceptions
{
    public class FailedMoveStorageException : Xeption
    {
        public FailedMoveStorageException(Exception innerException)
            : base(message: "Failed move storage error occurred, contact support.", innerException)
        { }
    }
}
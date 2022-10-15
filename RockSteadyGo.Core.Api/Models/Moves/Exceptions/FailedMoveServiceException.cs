// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Moves.Exceptions
{
    public class FailedMoveServiceException : Xeption
    {
        public FailedMoveServiceException(Exception innerException)
            : base(message: "Failed move service occurred, please contact support", innerException)
        { }
    }
}
// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Moves.Exceptions
{
    public class MoveServiceException : Xeption
    {
        public MoveServiceException(Exception innerException)
            : base(message: "Move service error occurred, contact support.", innerException)
        { }
    }
}
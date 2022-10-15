// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Moves.Exceptions
{
    public class AlreadyExistsMoveException : Xeption
    {
        public AlreadyExistsMoveException(Exception innerException)
            : base(message: "Move with the same Id already exists.", innerException)
        { }
    }
}
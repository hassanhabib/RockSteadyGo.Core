// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Moves.Exceptions
{
    public class LockedMoveException : Xeption
    {
        public LockedMoveException(Exception innerException)
            : base(message: "Locked move record exception, please try again later", innerException)
        {
        }
    }
}
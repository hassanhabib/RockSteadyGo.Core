// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Moves.Exceptions
{
    public class NotFoundMoveException : Xeption
    {
        public NotFoundMoveException(Guid moveId)
            : base(message: $"Couldn't find move with moveId: {moveId}.")
        { }
    }
}
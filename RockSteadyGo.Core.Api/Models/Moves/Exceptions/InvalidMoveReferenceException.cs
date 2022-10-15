// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Moves.Exceptions
{
    public class InvalidMoveReferenceException : Xeption
    {
        public InvalidMoveReferenceException(Exception innerException)
            : base(message: "Invalid move reference error occurred.", innerException) { }
    }
}
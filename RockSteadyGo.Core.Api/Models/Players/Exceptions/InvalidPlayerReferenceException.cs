// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Players.Exceptions
{
    public class InvalidPlayerReferenceException : Xeption
    {
        public InvalidPlayerReferenceException(Exception innerException)
            : base(message: "Invalid player reference error occurred.", innerException) { }
    }
}
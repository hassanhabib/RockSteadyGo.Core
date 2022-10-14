// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Matches.Exceptions
{
    public class InvalidMatchReferenceException : Xeption
    {
        public InvalidMatchReferenceException(Exception innerException)
            : base(message: "Invalid match reference error occurred.", innerException) { }
    }
}
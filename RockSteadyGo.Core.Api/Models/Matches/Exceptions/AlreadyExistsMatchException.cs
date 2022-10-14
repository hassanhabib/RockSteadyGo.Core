// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Matches.Exceptions
{
    public class AlreadyExistsMatchException : Xeption
    {
        public AlreadyExistsMatchException(Exception innerException)
            : base(message: "Match with the same Id already exists.", innerException)
        { }
    }
}
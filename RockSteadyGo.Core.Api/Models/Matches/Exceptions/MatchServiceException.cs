// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Matches.Exceptions
{
    public class MatchServiceException : Xeption
    {
        public MatchServiceException(Exception innerException)
            : base(message: "Match service error occurred, contact support.", innerException)
        { }
    }
}
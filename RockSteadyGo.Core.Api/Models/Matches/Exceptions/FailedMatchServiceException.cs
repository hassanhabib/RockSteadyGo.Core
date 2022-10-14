// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Matches.Exceptions
{
    public class FailedMatchServiceException : Xeption
    {
        public FailedMatchServiceException(Exception innerException)
            : base(message: "Failed match service occurred, please contact support", innerException)
        { }
    }
}
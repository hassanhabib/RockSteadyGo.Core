// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Matches.Exceptions
{
    public class FailedMatchStorageException : Xeption
    {
        public FailedMatchStorageException(Exception innerException)
            : base(message: "Failed match storage error occurred, contact support.", innerException)
        { }
    }
}
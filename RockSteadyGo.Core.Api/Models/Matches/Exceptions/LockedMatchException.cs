// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Matches.Exceptions
{
    public class LockedMatchException : Xeption
    {
        public LockedMatchException(Exception innerException)
            : base(message: "Locked match record exception, please try again later", innerException)
        {
        }
    }
}
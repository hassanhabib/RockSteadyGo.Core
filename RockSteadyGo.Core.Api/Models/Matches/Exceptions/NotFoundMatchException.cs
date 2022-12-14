// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Matches.Exceptions
{
    public class NotFoundMatchException : Xeption
    {
        public NotFoundMatchException(Guid matchId)
            : base(message: $"Couldn't find match with matchId: {matchId}.")
        { }
    }
}
// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Players.Exceptions
{
    public class AlreadyExistsPlayerException : Xeption
    {
        public AlreadyExistsPlayerException(Exception innerException)
            : base(message: "Player with the same Id already exists.", innerException)
        { }
    }
}